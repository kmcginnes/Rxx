using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	/// <summary>
	/// Provides extension and factory methods for <see cref="IPairedObserver{TLeft,TRight}"/>.
	/// </summary>
	public static class PairedObserver
	{
		/// <summary>
		/// Creates an observer that is capable of observing paired observables using the specified actions.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="onNextLeft">Handler for notifications from the left channel.</param>
		/// <param name="onNextRight">Handler for notifications from the right channel.</param>
		/// <returns>An observer capable of observing paired observables.</returns>
		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				ex =>
				{
					throw ex.PrepareForRethrow();
				},
				() => { });
		}

		/// <summary>
		/// Creates an observer that is capable of observing paired observables using the specified actions.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="onNextLeft">Handler for notifications from the left channel.</param>
		/// <param name="onNextRight">Handler for notifications from the right channel.</param>
		/// <param name="onError">Handler for an error notification.</param>
		/// <returns>An observer capable of observing paired observables.</returns>
		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				onError,
				() => { });
		}

		/// <summary>
		/// Creates an observer that is capable of observing paired observables using the specified actions.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="onNextLeft">Handler for notifications from the left channel.</param>
		/// <param name="onNextRight">Handler for notifications from the right channel.</param>
		/// <param name="onCompleted">Handler for a completed notification.</param>
		/// <returns>An observer capable of observing paired observables.</returns>
		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action onCompleted)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				ex =>
				{
					throw ex.PrepareForRethrow();
				},
				onCompleted);
		}

		/// <summary>
		/// Creates an observer that is capable of observing paired observables using the specified actions.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="onNextLeft">Handler for notifications from the left channel.</param>
		/// <param name="onNextRight">Handler for notifications from the right channel.</param>
		/// <param name="onError">Handler for an error notification.</param>
		/// <param name="onCompleted">Handler for a completed notification.</param>
		/// <returns>An observer capable of observing paired observables.</returns>
		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError,
			Action onCompleted)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			var observer = Observer.Create<Either<TLeft, TRight>>(
				value => value.Switch(onNextLeft, onNextRight),
				onError,
				onCompleted);

			Contract.Assume(observer != null);

			return new AnonymousPairedObserver<TLeft, TRight>(observer);
		}
	}
}
