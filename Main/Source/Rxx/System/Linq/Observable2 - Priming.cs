using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;
using System.Threading;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> Prime<TSource>(this IConnectableObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return Prime(source, _ => { });
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", 
			Justification = "Connection can be disposed by the whenConnected action.")]
		public static IObservable<TSource> Prime<TSource>(
			this IConnectableObservable<TSource> source,
			Action<IDisposable> whenConnected)
		{
			Contract.Requires(source != null);
			Contract.Requires(whenConnected != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			object gate = new object();
			bool isFirst = true;

			var observable = Observable.CreateWithDisposable<TSource>(
				observer =>
				{
					var subscription = source.Subscribe(observer);

					lock (gate)
					{
						if (isFirst)
						{
							isFirst = false;

							var connection = source.Connect();

							whenConnected(Disposable.Create(() =>
							{
								lock (gate)
								{
									if (!isFirst)
									{
										connection.Dispose();

										isFirst = true;
									}
								}
							}));
						}
					}

					return subscription;
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<Unit> StartPrimed(Action action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			Contract.Assume(Scheduler.NewThread != null);

			return StartPrimed(action, Scheduler.NewThread);
		}

		public static IObservable<Unit> StartPrimed(Action action, IScheduler scheduler)
		{
			Contract.Requires(action != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			int isConnected = 0;
			IObservable<Unit> startObservable = null;

			var observable = Observable.CreateWithDisposable<Unit>(
				observer =>
				{
					if (Interlocked.Exchange(ref isConnected, 1) == 0)
					{
						startObservable = Observable.Start(action, scheduler);
					}

					return startObservable.Subscribe(observer);
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TSource> StartPrimed<TSource>(Func<TSource> function)
		{
			Contract.Requires(function != null);
			Contract.Ensures(Contract.Result<IObservable<Unit>>() != null);

			Contract.Assume(Scheduler.NewThread != null);

			return StartPrimed(function, Scheduler.NewThread);
		}

		public static IObservable<TSource> StartPrimed<TSource>(Func<TSource> function, IScheduler scheduler)
		{
			Contract.Requires(function != null);
			Contract.Requires(scheduler != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			int isConnected = 0;
			IObservable<TSource> startObservable = null;

			var observable = Observable.CreateWithDisposable<TSource>(
				observer =>
				{
					if (Interlocked.Exchange(ref isConnected, 1) == 0)
					{
						startObservable = Observable.Start(function, scheduler);
					}

					return startObservable.Subscribe(observer);
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
