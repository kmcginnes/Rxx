using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Rxx.Properties;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IPairedObservable<TLeft, TRight> Pair<TLeft, TRight>(
			this IObservable<TLeft> leftSource,
			Func<TLeft, TRight> rightSelector)
		{
			Contract.Requires(leftSource != null);
			Contract.Requires(rightSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					return leftSource.Subscribe(
						left =>
						{
							observer.OnNextLeft(left);
							observer.OnNextRight(rightSelector(left));
						},
						observer.OnError,
						observer.OnCompleted);
				});
		}

		public static IPairedObservable<TLeft, TRight> Pair<TLeft, TRight>(
			this IObservable<TLeft> leftSource,
			IObservable<TRight> rightSource)
		{
			Contract.Requires(leftSource != null);
			Contract.Requires(rightSource != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return Pair(leftSource, rightSource, (left, right) => PairDirection.Both);
		}

		public static IPairedObservable<TLeft, TRight> Pair<TLeft, TRight>(
			this IObservable<TLeft> leftSource,
			IObservable<TRight> rightSource,
			Func<TLeft, TRight, PairDirection> directionSelector)
		{
			Contract.Requires(leftSource != null);
			Contract.Requires(rightSource != null);
			Contract.Requires(directionSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
					leftSource.Maybe()
					.CombineLatest(
						rightSource.Maybe(),
						(left, right) => new
						{
							left,
							right
						})
					.Subscribe(
						pair =>
						{
							if (!pair.left.HasValue)
							{
								if (pair.right.HasValue)
								{
									observer.OnNextRight(pair.right.Value);
								}
							}
							else if (!pair.right.HasValue)
							{
								observer.OnNextLeft(pair.left.Value);
							}
							else
							{
								var left = pair.left.Value;
								var right = pair.right.Value;

								switch (directionSelector(left, right))
								{
									case PairDirection.Left:
										observer.OnNextLeft(left);
										break;
									case PairDirection.Right:
										observer.OnNextRight(right);
										break;
									case PairDirection.Both:
										observer.OnNextLeft(left);
										observer.OnNextRight(right);
										break;
									case PairDirection.Neither:
										break;
									default:
										throw new InvalidOperationException(Errors.InvalidPairDirectionValue);
								}
							}
						},
						observer.OnError,
						observer.OnCompleted));
		}

		public static IPairedObservable<TLeft, TRight> AsPairedObservable<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					return source.Subscribe(observer);
				});
		}
	}
}
