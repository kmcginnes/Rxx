using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class Observable2
	{
		private static IDisposable Notify<T>(IObserver<T> observer, T value, TimeSpan period, IScheduler scheduler)
		{
			Contract.Requires(observer != null);
			Contract.Requires(scheduler != null);

			if (period <= TimeSpan.Zero)
			{
				observer.OnNext(value);
				observer.OnCompleted();
			}
			else
			{
				var now = scheduler.Now;

				observer.OnNext(value);

				period -= scheduler.Now - now;

				if (period <= TimeSpan.Zero)
				{
					observer.OnCompleted();
				}
				else
				{
					return scheduler.Schedule(observer.OnCompleted, period);
				}
			}

			return Disposable.Empty;
		}

		public static IObservable<Unit> Delay(TimeSpan dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime);
		}

		public static IObservable<Unit> Delay(TimeSpan dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, scheduler);
		}

		public static IObservable<Unit> Delay(TimeSpan dueTime, TimeSpan period)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, period);
		}

		public static IObservable<Unit> Delay(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, period, scheduler);
		}

		public static IObservable<Unit> Delay(DateTimeOffset dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime);
		}

		public static IObservable<Unit> Delay(DateTimeOffset dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, scheduler);
		}

		public static IObservable<Unit> Delay(DateTimeOffset dueTime, TimeSpan period)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, period);
		}

		public static IObservable<Unit> Delay(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(new Unit(), dueTime, period, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, TimeSpan dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return Delay(value, dueTime, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, TimeSpan dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(value, dueTime, TimeSpan.Zero, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, TimeSpan dueTime, TimeSpan period)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return Delay(value, dueTime, period, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var observable = Observable.CreateWithDisposable<T>(
				observer =>
				{
					var disposables = new CompositeDisposable();

					disposables.Add(
						scheduler.Schedule(
							() => disposables.Add(Notify(observer, value, period, scheduler)),
							dueTime));

					return disposables;
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> Delay<T>(T value, DateTimeOffset dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return Delay(value, dueTime, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, DateTimeOffset dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return Delay(value, dueTime, TimeSpan.Zero, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, DateTimeOffset dueTime, TimeSpan period)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return Delay(value, dueTime, period, scheduler);
		}

		public static IObservable<T> Delay<T>(T value, DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var observable = Observable.CreateWithDisposable<T>(
				observer =>
				{
					var disposables = new CompositeDisposable();

					disposables.Add(
						scheduler.Schedule(
							() => disposables.Add(Notify(observer, value, period, scheduler)),
							dueTime));

					return disposables;
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<Unit> DelayOnCompleted(TimeSpan dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return DelayOnCompleted<Unit>(dueTime);
		}

		public static IObservable<Unit> DelayOnCompleted(TimeSpan dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return DelayOnCompleted<Unit>(dueTime, scheduler);
		}

		public static IObservable<Unit> DelayOnCompleted(DateTimeOffset dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return DelayOnCompleted<Unit>(dueTime);
		}

		public static IObservable<Unit> DelayOnCompleted(DateTimeOffset dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			return DelayOnCompleted<Unit>(dueTime, scheduler);
		}

		public static IObservable<T> DelayOnCompleted<T>(TimeSpan dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return DelayOnCompleted<T>(dueTime, scheduler);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Disposables are composited by the outer observable.")]
		public static IObservable<T> DelayOnCompleted<T>(TimeSpan dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var observable = Observable.CreateWithDisposable<T>(
				observer =>
				{
					var disposables = new CompositeDisposable();

					disposables.Add(
						scheduler.Schedule(
							observer.OnCompleted,
							dueTime));

					return disposables;
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> DelayOnCompleted<T>(DateTimeOffset dueTime)
		{
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var scheduler = Scheduler.ThreadPool;

			Contract.Assume(scheduler != null);

			return DelayOnCompleted<T>(dueTime, scheduler);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Disposables are composited by the outer observable.")]
		public static IObservable<T> DelayOnCompleted<T>(DateTimeOffset dueTime, IScheduler scheduler)
		{
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			var observable = Observable.CreateWithDisposable<T>(
				observer =>
				{
					var disposables = new CompositeDisposable();

					disposables.Add(
						scheduler.Schedule(
							observer.OnCompleted,
							dueTime));

					return disposables;
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
