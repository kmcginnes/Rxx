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

			var observable = source
				.Select(value => Observable.Return(value).Delay(minimumPeriod, scheduler))
				.Concat();

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

			var observable = source
				.Select(value => Observable.Empty<TSource>().Delay(minimumPeriod, scheduler).StartWith(value))
				.Concat();

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
