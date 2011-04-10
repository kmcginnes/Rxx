using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		public static IPairedObservable<TLeft, TRight> Create<TLeft, TRight>(
			Func<IPairedObserver<TLeft, TRight>, Action> subscribe)
		{
			Contract.Requires(subscribe != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return CreateWithDisposable<TLeft, TRight>(o => Disposable.Create(subscribe(o)));
		}

		public static IPairedObservable<TLeft, TRight> CreateWithDisposable<TLeft, TRight>(
			Func<IPairedObserver<TLeft, TRight>, IDisposable> subscribe)
		{
			Contract.Requires(subscribe != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			var observable = Observable.CreateWithDisposable<Either<TLeft, TRight>>(
				observer =>
				{
					return subscribe(new AnonymousPairedObserver<TLeft, TRight>(observer));
				});

			Contract.Assume(observable != null);

			return new AnonymousPairedObservable<TLeft, TRight>(observable);
		}
	}
}
