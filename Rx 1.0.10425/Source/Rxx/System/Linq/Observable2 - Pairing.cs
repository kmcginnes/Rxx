using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Rxx.Properties;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <summary>
		/// Converts the specified observable sequence of <see cref="Either{TLeft,TRight}"/> into
		/// an <see cref="IPairedObservable{TLeft,TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="source">The observable sequence to convert.</param>
		/// <returns>The specified observable sequence as a paired observable.</returns>
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

		/// <summary>
		/// Creates a paired observable from the specified observable sequence and selector function.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="leftSource">The observable sequence that provides notifications for the left channel.</param>
		/// <param name="rightSelector">Selects a value for the right channel from each value in the specified observable sequence.</param>
		/// <returns>The specified observable sequence paired with the values produced by the selector.</returns>
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

		/// <summary>
		/// Creates a paired observable by combining the latest values of the specified observable sequences.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="leftSource">The observable sequence that provides notifications for the left channel.</param>
		/// <param name="rightSource">The observable sequence that provides notifications for the right channel.</param>
		/// <returns>The latest vlaues of both observable sequences paired together.</returns>
		public static IPairedObservable<TLeft, TRight> Pair<TLeft, TRight>(
			this IObservable<TLeft> leftSource,
			IObservable<TRight> rightSource)
		{
			Contract.Requires(leftSource != null);
			Contract.Requires(rightSource != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return Pair(leftSource, rightSource, (left, right) => PairDirection.Both);
		}

		/// <summary>
		/// Creates a paired observable by combining the latest values of the specified observable sequences
		/// and choosing which channels will receive values based on the specified selector.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="leftSource">The observable sequence that provides notifications for the left channel.</param>
		/// <param name="rightSource">The observable sequence that provides notifications for the right channel.</param>
		/// <param name="directionSelector">Selects the channels that will receive notifications for every pair.</param>
		/// <returns>The specified observable sequences paired together and modified by the specified selector.</returns>
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
	}
}
