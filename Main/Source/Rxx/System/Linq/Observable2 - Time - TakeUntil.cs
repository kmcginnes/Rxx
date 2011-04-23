using System.Concurrency;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, DateTimeOffset stopTime)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = source.TakeUntil(Delay(stopTime));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, DateTimeOffset stopTime, IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = source.TakeUntil(Delay(stopTime, scheduler));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, TimeSpan duration)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = source.TakeUntil(Delay(duration));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
		{
			Contract.Requires(source != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = source.TakeUntil(Delay(duration, scheduler));

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
