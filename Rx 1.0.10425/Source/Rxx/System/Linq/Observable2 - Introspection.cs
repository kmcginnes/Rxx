using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <summary>
		/// Pairs the specified observable sequence with an observable for each value that indicates 
		/// the duration of the observation of that value.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence to introspect.</param>
		/// <returns>A paired observable with the left channel providing introspection windows and the 
		/// right channel providing values from the specified observable.</returns>
		public static IPairedObservable<IObservable<TSource>, TSource> Introspect<TSource>(
			this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IPairedObservable<IObservable<TSource>, TSource>>() != null);

			var scheduler = Scheduler.Immediate;

			Contract.Assume(scheduler != null);

			return Introspect(source, scheduler);
		}

		/// <summary>
		/// Pairs the specified observable sequence with an observable for each value that indicates 
		/// the duration of the observation of that value.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence to introspect.</param>
		/// <param name="scheduler">Schedules the observations of values in the right notification channel.</param>
		/// <returns>A paired observable with the left channel providing introspection windows and the 
		/// right channel providing values from the specified observable.</returns>
		public static IPairedObservable<IObservable<TSource>, TSource> Introspect<TSource>(
			this IObservable<TSource> source,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IPairedObservable<IObservable<TSource>, TSource>>() != null);

			return PairedObservable.CreateWithDisposable<IObservable<TSource>, TSource>(
				observer =>
				{
					var observations = new Subject<Tuple<TSource, ISubject<TSource>>>(scheduler);

					int pendingOnNext = 0;
					bool sourceCompleted = false;
					object gate = new object();

					var observationsSubscription = observations.Subscribe(
						next =>
						{
							var value = next.Item1;
							var introspection = next.Item2;

							try
							{
								observer.OnNextRight(value);
							}
							catch (Exception ex)
							{
								introspection.OnError(ex);
								return;
							}

							introspection.OnCompleted();

							bool completeNow = false;

							lock (gate)
							{
								if (--pendingOnNext == 0 && sourceCompleted)
								{
									completeNow = true;
								}
							}

							if (completeNow)
							{
								observer.OnCompleted();
							}
						},
						observer.OnError,
						observer.OnCompleted);

					var sourceSubscription = source.Subscribe(
						value =>
						{
							var introspection = new ReplaySubject<TSource>(1);

							observer.OnNextLeft(introspection.AsObservable());

							introspection.OnNext(value);

							lock (gate)
							{
								pendingOnNext++;
							}

							observations.OnNext(Tuple.Create(value, (ISubject<TSource>) introspection));
						},
						observations.OnError,
						() =>
						{
							bool completeNow = false;

							lock (gate)
							{
								sourceCompleted = true;
								completeNow = pendingOnNext == 0;
							}

							if (completeNow)
							{
								observations.OnCompleted();
							}
						});

					return new CompositeDisposable(sourceSubscription, observationsSubscription);
				});
		}

		/// <summary>
		/// Generates a sequence of windows where each window contains all values that were observed from 
		/// the <paramref name="source"/> while the values in the previous window were being observed.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence from which to create introspection windows.</param>
		/// <returns>The source observable sequence buffered into introspection windows.</returns>
		public static IObservable<IObservable<TSource>> WindowIntrospective<TSource>(
			this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<IObservable<TSource>>>() != null);

			var scheduler = Scheduler.TaskPool;

			Contract.Assume(scheduler != null);

			return WindowIntrospective(source, scheduler);
		}

		/// <summary>
		/// Generates a sequence of windows where each window contains all values that were observed from 
		/// the <paramref name="source"/> while the values in the previous window were being observed.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence from which to create introspection windows.</param>
		/// <param name="scheduler">Schedules when windows are observed as well as the values in each window.</param>
		/// <returns>The source observable sequence buffered into introspection windows.</returns>
		public static IObservable<IObservable<TSource>> WindowIntrospective<TSource>(
			this IObservable<TSource> source,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<IObservable<TSource>>>() != null);

			var observable = Observable.CreateWithDisposable<IObservable<TSource>>(
				observer =>
				{
					var queue = new Queue<TSource>();
					var window = new Subject<TSource>();

					bool pendingDrain = false;
					bool sourceCompleted = false;
					object gate = new object();

					var sourceSubscription = new MutableDisposable();
					var drainSchedule = new MutableDisposable();
					var schedules = new CompositeDisposable(drainSchedule);

					Action ensureDraining = () =>
						{
							if (pendingDrain)
								return;

							pendingDrain = true;

							drainSchedule.Disposable =
								scheduler.Schedule(self =>
								{
									Queue<TSource> currentQueue;

									lock (gate)
									{
										currentQueue = queue;
										queue = new Queue<TSource>();
									}

									currentQueue.Run(window.OnNext);

									window.OnCompleted();

									Contract.Assume(pendingDrain);

									bool loop, completeNow;

									lock (gate)
									{
										pendingDrain = queue.Count > 0;
										completeNow = !pendingDrain && sourceCompleted;

										if (completeNow)
										{
											loop = false;
										}
										else
										{
											window = new Subject<TSource>();

											// Must push the new window before unlocking the gate to avoid a race condition when pendingDrain is false.
											observer.OnNext(window);

											// Ensure pendingDrain is read again after making a call to OnNext, in case of re-entry.
											loop = pendingDrain;
										}
									}

									if (completeNow)
									{
										observer.OnCompleted();
									}
									else if (loop)
									{
										Contract.Assert(pendingDrain);

										self();
									}
								});
						};

					schedules.Add(
						scheduler.Schedule(() =>
							{
								observer.OnNext(window);

								sourceSubscription.Disposable = source.Subscribe(
									value =>
									{
										lock (gate)
										{
											queue.Enqueue(value);

											ensureDraining();
										}
									},
									ex => schedules.Add(scheduler.Schedule(() => observer.OnError(ex))),
									() =>
									{
										bool completeNow = false;

										lock (gate)
										{
											sourceCompleted = true;
											completeNow = !pendingDrain;
										}

										if (completeNow)
										{
											schedules.Add(scheduler.Schedule(() =>
												{
													window.OnCompleted();

													observer.OnCompleted();
												}));
										}
									});
							}));

					return new CompositeDisposable(sourceSubscription, drainSchedule, schedules);
				});

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Generates a sequence of lists where each list contains all values that were observed from 
		/// the <paramref name="source"/> while the previous list was being observed.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence from which to create introspection lists.</param>
		/// <returns>The source observable sequence buffered into introspection lists.</returns>
		public static IObservable<IList<TSource>> BufferIntrospective<TSource>(
			this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<IList<TSource>>>() != null);

			var observable = from window in source.WindowIntrospective()
											 from list in window.ToList()
											 select list;

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Generates a sequence of lists where each list contains all values that were observed from 
		/// the <paramref name="source"/> while the previous list was being observed.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable sequence from which to create introspection lists.</param>
		/// <param name="scheduler">Schedules when lists are observed.</param>
		/// <returns>The source observable sequence buffered into introspection lists.</returns>
		public static IObservable<IList<TSource>> BufferIntrospective<TSource>(
			this IObservable<TSource> source,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<IList<TSource>>>() != null);

			var observable = from window in source.WindowIntrospective(scheduler)
											 from list in window.ToList()
											 select list;

			Contract.Assume(observable != null);

			return observable;
		}
	}
}