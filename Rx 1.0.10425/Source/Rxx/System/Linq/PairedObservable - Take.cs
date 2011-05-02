using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	/// <summary>
	/// Provides extension and factory methods for <see cref="IPairedObservable{TLeft,TRight}"/>.
	/// </summary>
	public static partial class PairedObservable
	{
		/// <summary>
		/// Returns an observable that contains only the values from the left notification channel.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="source">The observable from which values are taken.</param>
		/// <returns>An observable of values from the left notification channel.</returns>
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

		/// <summary>
		/// Returns an observable that contains only the values from the right notification channel.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="source">The observable from which values are taken.</param>
		/// <returns>An observable of values from the right notification channel.</returns>
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

		/// <summary>
		/// Returns an observable that contains only the values from the left notification channel
		/// up to the specified <paramref name="count"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="source">The observable from which values are taken.</param>
		/// <param name="count">The number of values to take.</param>
		/// <returns>An observable of values from the left notification channel.</returns>
		public static IPairedObservable<TLeft, TRight> TakeLeft<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			int count)
		{
			Contract.Requires(source != null);
			Contract.Requires(count >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					int remaining = count;

					return source.Subscribe(
						left =>
						{
							if (remaining > 0)
							{
								remaining--;

								observer.OnNextLeft(left);

								if (remaining == 0)
								{
									observer.OnCompleted();
								}
							}
						},
						observer.OnNextRight,
						observer.OnError,
						observer.OnCompleted);
				});
		}

		/// <summary>
		/// Returns an observable that contains only the values from the right notification channel
		/// up to the specified <paramref name="count"/>.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="source">The observable from which values are taken.</param>
		/// <param name="count">The number of values to take.</param>
		/// <returns>An observable of values from the right notification channel.</returns>
		public static IPairedObservable<TLeft, TRight> TakeRight<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			int count)
		{
			Contract.Requires(source != null);
			Contract.Requires(count >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					int remaining = count;

					return source.Subscribe(
						observer.OnNextLeft,
						right =>
						{
							if (remaining > 0)
							{
								remaining--;

								observer.OnNextRight(right);

								if (remaining == 0)
								{
									observer.OnCompleted();
								}
							}
						},
						observer.OnError,
						observer.OnCompleted);
				});
		}
	}
}
