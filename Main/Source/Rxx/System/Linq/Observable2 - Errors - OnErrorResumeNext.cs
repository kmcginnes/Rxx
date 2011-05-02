using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <summary>
		/// Moves to the next observable sequence when the current sequence throws the specified type of exception.
		/// The output is paired with an error channel.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <typeparam name="TException">The type of exception to catch.</typeparam>
		/// <param name="sources">The observables to be enumerated.</param>
		/// <returns>An observable sequence with an error channel.</returns>
		public static IPairedObservable<TSource, TException> OnErrorResumeNext<TSource, TException>(
			this IEnumerable<IObservable<TSource>> sources)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			var observable = Observable.Defer<Either<TSource, TException>>(() =>
			{
				var cursor = sources.GetEnumerator();

				return OnErrorResumeNext<TSource, TException>(
					cursor,
					ex => cursor,
					ex => TimeSpan.Zero);
			});

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		/// <summary>
		/// Moves to the next observable sequence when the current sequence throws the specified type of exception 
		/// using the specified back-off algorithm.  The output is paired with an error channel.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <typeparam name="TException">The type of exception to catch.</typeparam>
		/// <param name="sources">The observables to be enumerated.</param>
		/// <param name="backOffSelector">Selects the amount of time to delay before moving to the next observable 
		/// when the current sequence has faulted.</param>
		/// <returns>An observable sequence with an error channel.</returns>
		public static IPairedObservable<TSource, TException> OnErrorResumeNext<TSource, TException>(
			this IEnumerable<IObservable<TSource>> sources,
			Func<TException, TimeSpan> backOffSelector)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			var observable = Observable.Defer<Either<TSource, TException>>(() =>
			{
				var cursor = sources.GetEnumerator();

				return OnErrorResumeNext<TSource, TException>(
					cursor,
					ex => cursor,
					backOffSelector);
			});

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		/// <summary>
		/// Moves to the next observable sequence when the current sequence throws the specified type of exception.
		/// The output is paired with an error channel.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <typeparam name="TException">The type of exception to catch.</typeparam>
		/// <param name="sources">The observables to be enumerated.</param>
		/// <returns>An observable sequence with an error channel.</returns>
		public static IPairedObservable<TSource, TException> OnErrorResumeNext<TSource, TException>(
			this IEnumerator<IObservable<TSource>> sources)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return OnErrorResumeNext<TSource, TException>(sources, ex => sources, ex => TimeSpan.Zero);
		}

		/// <summary>
		/// Moves to the next observable sequence provided by the specified <paramref name="handler"/> when the current 
		/// sequence throws the specified type of exception.  The output is paired with an error channel.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <typeparam name="TException">The type of exception to catch.</typeparam>
		/// <param name="sources">The observables to be enumerated.</param>
		/// <param name="handler">Selects the next enumerator when an observable from the current enumerator has faulted.</param>
		/// <returns>An observable sequence with an error channel.</returns>
		public static IPairedObservable<TSource, TException> OnErrorResumeNext<TSource, TException>(
			this IEnumerator<IObservable<TSource>> sources,
			Func<TException, IEnumerator<IObservable<TSource>>> handler)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Requires(handler != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return OnErrorResumeNext<TSource, TException>(sources, handler, ex => TimeSpan.Zero);
		}

		/// <summary>
		/// Moves to the next observable sequence provided by the specified <paramref name="handler"/> when the current 
		/// sequence throws the specified type of exception using the specified back-off algorithm.
		/// The output is paired with an error channel.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <typeparam name="TException">The type of exception to catch.</typeparam>
		/// <param name="sources">The observables to be enumerated.</param>
		/// <param name="handler">Selects the next enumerator when an observable from the current enumerator has faulted.</param>
		/// <param name="backOffSelector">Selects the amount of time to delay before moving to the next observable 
		/// when the current sequence has faulted.</param>
		/// <returns>An observable sequence with an error channel.</returns>
		public static IPairedObservable<TSource, TException> OnErrorResumeNext<TSource, TException>(
			this IEnumerator<IObservable<TSource>> sources,
			Func<TException, IEnumerator<IObservable<TSource>>> handler,
			Func<TException, TimeSpan> backOffSelector)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Requires(handler != null);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return PairedObservable.CreateWithDisposable<TSource, TException>(
				observer =>
				{
					bool movedNext;
					IObservable<TSource> current = null;

					Func<bool> moveNext = () =>
					{
						try
						{
							movedNext = sources.MoveNext();

							if (movedNext)
							{
								current = sources.Current;

								Contract.Assume(current != null);

								return true;
							}
						}
						catch (Exception ex)
						{
							observer.OnError(ex);

							return false;
						}

						return false;
					};

					if (!moveNext())
					{
						observer.OnCompleted();

						return sources;
					}

					var subscription = new MutableDisposable();
					var sourcesDisposable = new MutableDisposable();

					sourcesDisposable.Disposable = sources;

					var disposable = Scheduler.CurrentThread.Schedule(
						self =>
						{
							subscription.SetDisposableIndirectly(() =>
								current.Subscribe(
									observer.OnNextLeft,
									ex =>
									{
										var typedError = ex as TException;

										if (typedError == null)
										{
											observer.OnError(ex);
										}
										else
										{
											observer.OnNextRight(typedError);

											IEnumerator<IObservable<TSource>> next;

											try
											{
												next = handler(typedError);
											}
											catch (Exception ex2)
											{
												observer.OnError(ex2);
												return;
											}

											Contract.Assume(next != null);

											if (sources != next)
											{
												sources = next;

												sourcesDisposable.Disposable = sources;
											}

											if (moveNext())
											{
												TimeSpan delay;

												try
												{
													delay = backOffSelector(typedError);
												}
												catch (Exception ex2)
												{
													observer.OnError(ex2);
													return;
												}

												if (delay < TimeSpan.Zero)
												/* Feature that allows callers to indicate when an exception is fatal based on its type */
												{
													observer.OnError(ex);
												}
												else
												{
													self(delay);
												}
											}
											else
											{
												observer.OnCompleted();
											}
										}
									},
									() =>
									{
										if (moveNext())
										{
											self(TimeSpan.Zero);
										}
										else
										{
											observer.OnCompleted();
										}
									}));
						},
						TimeSpan.Zero);

					return new CompositeDisposable(subscription, disposable, sourcesDisposable);
				});
		}
	}
}
