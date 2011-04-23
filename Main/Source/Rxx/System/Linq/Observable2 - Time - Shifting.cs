using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, TimeSpan> dueTimeSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeSelector != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(value => Tuple.Create(dueTimeSelector(value), TimeSpan.Zero));
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, TimeSpan> dueTimeSelector,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeSelector != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(
				value => Tuple.Create(dueTimeSelector(value), TimeSpan.Zero),
				scheduler);
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, Tuple<TimeSpan, TimeSpan>> dueTimeAndPeriodSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeAndPeriodSelector != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return source.TimeShift(dueTimeAndPeriodSelector, scheduler);
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, Tuple<TimeSpan, TimeSpan>> dueTimeAndPeriodSelector,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeAndPeriodSelector != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(
				value =>
				{
					var now = scheduler.Now;

					var delays = dueTimeAndPeriodSelector(value);

					TimeSpan dueTime = delays.Item1;
					TimeSpan period = delays.Item2;

					dueTime -= scheduler.Now - now;

					return Delay(dueTime, period, scheduler);
				});
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, DateTimeOffset> dueTimeSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeSelector != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(value => Tuple.Create(dueTimeSelector(value), TimeSpan.Zero));
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, DateTimeOffset> dueTimeSelector,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeSelector != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(
				value => Tuple.Create(dueTimeSelector(value), TimeSpan.Zero),
				scheduler);
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, Tuple<DateTimeOffset, TimeSpan>> dueTimeAndPeriodSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeAndPeriodSelector != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return source.TimeShift(dueTimeAndPeriodSelector, scheduler);
		}

		public static IObservable<TSource> TimeShift<TSource>(
			this IObservable<TSource> source,
			Func<TSource, Tuple<DateTimeOffset, TimeSpan>> dueTimeAndPeriodSelector,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(dueTimeAndPeriodSelector != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return source.TimeShift(
				value =>
				{
					var delays = dueTimeAndPeriodSelector(value);

					DateTimeOffset dueTime = delays.Item1;
					TimeSpan period = delays.Item2;

					return Delay(dueTime, period, scheduler);
				});
		}

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
