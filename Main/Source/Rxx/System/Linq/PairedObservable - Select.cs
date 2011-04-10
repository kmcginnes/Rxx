using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		public static IPairedObservable<TLeftResult, TRightResult> Select<TLeft, TRight, TLeftResult, TRightResult>(
			this IObservable<Either<TLeft, TRight>> source,
			Func<TLeft, TLeftResult> leftSelector,
			Func<TRight, TRightResult> rightSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(leftSelector != null);
			Contract.Requires(rightSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeftResult, TRightResult>>() != null);

			return PairedObservable.CreateWithDisposable<TLeftResult, TRightResult>(
				observer =>
				{
					return source.Subscribe(
						left => observer.OnNextLeft(leftSelector(left)),
						right => observer.OnNextRight(rightSelector(right)),
						observer.OnError,
						observer.OnCompleted);
				});
		}

		public static IObservable<TResult> SelectLeft<TLeft, TRight, TResult>(
			this IObservable<Either<TLeft, TRight>> source,
			Func<TLeft, TResult> selector)
		{
			Contract.Requires(source != null);
			Contract.Requires(selector != null);
			Contract.Ensures(Contract.Result<IObservable<TResult>>() != null);

			var observable = source.TakeLeft().Select(selector);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TResult> SelectRight<TLeft, TRight, TResult>(
			this IObservable<Either<TLeft, TRight>> source,
			Func<TRight, TResult> selector)
		{
			Contract.Requires(source != null);
			Contract.Requires(selector != null);
			Contract.Ensures(Contract.Result<IObservable<TResult>>() != null);

			var observable = source.TakeRight().Select(selector);

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
