using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IPairedObservable<TSource, TException> Catch<TSource, TException>(
			this IEnumerable<IObservable<TSource>> sources)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			var observable = Observable.Defer<Either<TSource, TException>>(() =>
				{
					var cursor = sources.GetEnumerator();

					return Catch<TSource, TException>(
						cursor,
						ex => cursor,
						ex => TimeSpan.Zero);
				});

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IPairedObservable<TSource, TException> Catch<TSource, TException>(
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

					return Catch<TSource, TException>(
						cursor,
						ex => cursor,
						backOffSelector);
				});

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IPairedObservable<TSource, TException> Catch<TSource, TException>(
			this IEnumerator<IObservable<TSource>> sources)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return Catch<TSource, TException>(sources, ex => sources, ex => TimeSpan.Zero);
		}

		public static IPairedObservable<TSource, TException> Catch<TSource, TException>(
			this IEnumerator<IObservable<TSource>> sources,
			Func<TException, IEnumerator<IObservable<TSource>>> handler)
			where TException : Exception
		{
			Contract.Requires(sources != null);
			Contract.Requires(handler != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return Catch<TSource, TException>(sources, handler, ex => TimeSpan.Zero);
		}

		public static IPairedObservable<TSource, TException> Catch<TSource, TException>(
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
												observer.OnError(ex);
											}
										}
									},
									observer.OnCompleted));
						},
						TimeSpan.Zero);

					return new CompositeDisposable(subscription, disposable, sourcesDisposable);
				});
		}
	}
}
