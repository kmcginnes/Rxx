using System.Concurrency;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> AsInterval<TSource>(
			this IObservable<TSource> source,
			TimeSpan minimumPeriod)
		{
			Contract.Requires(source != null);
			Contract.Requires(minimumPeriod >= TimeSpan.Zero);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return AsInterval(source, minimumPeriod, scheduler);
		}

		public static IObservable<TSource> AsInterval<TSource>(
			this IObservable<TSource> source,
			TimeSpan minimumPeriod,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(minimumPeriod >= TimeSpan.Zero);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = Observable.Defer(() =>
			{
				bool first = true;
				var now = default(DateTimeOffset);
				var total = TimeSpan.Zero;

				return source.TimeShift(
					_ =>
					{
						var previous = now;
						now = scheduler.Now;

						if (first)
						{
							first = false;
							total += minimumPeriod;
						}
						else
						{
							total += minimumPeriod - (now - previous);

							if (total < TimeSpan.Zero)
							/* Delays greater than the minimum period must restart the 
							 * timer to maintain the minimum period; otherwise, the next
							 * value will be pushed at the originally calculated offset
							 * causing these two consecutive values to be observed in 
							 * less time than the minimum period.
							 */
							{
								total = TimeSpan.Zero;
							}
						}

						return Observable.Timer(now + total, scheduler);
					});
			});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> AsTimer<TSource>(
			this IObservable<TSource> source,
			TimeSpan minimumPeriod)
		{
			Contract.Requires(source != null);
			Contract.Requires(minimumPeriod >= TimeSpan.Zero);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return AsTimer(source, minimumPeriod, scheduler);
		}

		public static IObservable<TSource> AsTimer<TSource>(
			this IObservable<TSource> source,
			TimeSpan minimumPeriod,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(minimumPeriod >= TimeSpan.Zero);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = Observable.Defer(() =>
			{
				bool first = true;
				var now = default(DateTimeOffset);
				var total = TimeSpan.Zero;

				return source.TimeShift(
					_ =>
					{
						var previous = now;
						now = scheduler.Now;

						if (first)
						{
							first = false;
						}
						else
						{
							total += minimumPeriod - (now - previous);

							if (total < TimeSpan.Zero)
							/* Delays greater than the minimum period must restart the 
							 * timer to maintain the minimum period; otherwise, the next
							 * value will be pushed at the originally calculated offset
							 * causing these two consecutive values to be observed in 
							 * less time than the minimum period.
							 */
							{
								total = TimeSpan.Zero;
							}
						}

						return Observable.Timer(now + total, scheduler);
					});
			});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> Pulse<TSource>(
			this IObservable<TSource> source,
			TimeSpan period)
		{
			Contract.Requires(source != null);
			Contract.Requires(period >= TimeSpan.Zero);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return Pulse(source, period, scheduler);
		}

		public static IObservable<TSource> Pulse<TSource>(
			this IObservable<TSource> source,
			TimeSpan period,
			IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(period >= TimeSpan.Zero);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var timer = Observable.Timer(TimeSpan.Zero, period, scheduler);

			Contract.Assume(timer != null);

			return source.TimeShift(timer);
		}
	}
}