using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> TimeShift<TSource, TTimer>(
			this IObservable<TSource> source,
			Func<TSource, IObservable<TTimer>> timeSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(timeSelector != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = source.Publish(
				published => published.TimeShift(published.Select(timeSelector).Concat()));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> TimeShift<TSource, TTimer>(
			this IObservable<TSource> source,
			IObservable<TTimer> timer)
		{
			Contract.Requires(source != null);
			Contract.Requires(timer != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = Observable.CreateWithDisposable<TSource>(
				observer =>
				{
					var values = new Queue<TSource>();
					var last = System.Maybe<TSource>.Empty;

					object gate = new object();
					bool sourceCompleted = false;

					var sourceSubscription = source.Subscribe(
						value =>
						{
							lock (gate)
							{
								values.Enqueue(value);
							}
						},
						observer.OnError,
						() =>
						{
							bool completeNow = false;

							lock (gate)
							{
								sourceCompleted = true;

								if (values.Count == 0)
								{
									completeNow = true;
								}
							}

							if (completeNow)
							{
								observer.OnCompleted();
							}
						});

					var timerSubscription = timer.Subscribe(
						_ =>
						{
							bool completeNow = false;
							bool hasValue = false;

							var next = default(TSource);

							lock (gate)
							{
								if (values.Count > 0)
								{
									next = values.Dequeue();

									hasValue = true;
								}

								completeNow = sourceCompleted && values.Count == 0;
							}

							if (hasValue)
							{
								last = new Maybe<TSource>(next);

								observer.OnNext(next);
							}
							else if (!completeNow && last.HasValue)
							{
								observer.OnNext(last.Value);
							}

							if (completeNow)
							{
								observer.OnCompleted();
							}
						},
						observer.OnError,
						observer.OnCompleted);

					return new CompositeDisposable(sourceSubscription, timerSubscription);
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
