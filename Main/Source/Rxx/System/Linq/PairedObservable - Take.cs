using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		public static IObservable<TLeft> TakeLeft<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TLeft>>() != null);

			var observable = Observable.CreateWithDisposable<TLeft>(
				observer =>
				{
					return source.Subscribe(
						observer.OnNext,
						right => { },
						observer.OnError,
						observer.OnCompleted);
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TRight> TakeRight<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TRight>>() != null);

			var observable = Observable.CreateWithDisposable<TRight>(
				observer =>
				{
					return source.Subscribe(
						left => { },
						observer.OnNext,
						observer.OnError,
						observer.OnCompleted);
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
