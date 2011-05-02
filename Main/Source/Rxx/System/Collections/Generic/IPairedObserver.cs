namespace System.Collections.Generic
{
	/// <summary>
	/// Provides a mechanism for receiving push-based notifications from an <see cref="IPairedObservable{TLeft,TRight}"/>.
	/// </summary>
	/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
	/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
	public interface IPairedObserver<TLeft, TRight> : IObserver<Either<TLeft, TRight>>
	{
		/// <summary>
		/// Notifies left observers of a new value in the sequence.
		/// </summary>
		/// <param name="left">The current left notification information.</param>
		void OnNextLeft(TLeft left);

		/// <summary>
		/// Notifies right observers of a new value in the sequence.
		/// </summary>
		/// <param name="right">The current right notification information.</param>
		void OnNextRight(TRight right);
	}
}
