using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	/// <summary>
	/// Represents an observable with two notification channels for values.
	/// </summary>
	/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
	/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
	[ContractClass(typeof(IPairedObservableContract<,>))]
	public interface IPairedObservable<TLeft, TRight> : IObservable<Either<TLeft, TRight>>
	{
		/// <summary>
		/// Notifies the observable that an observer is to receive notifications.
		/// </summary>
		/// <param name="observer">The object that is to receive notifications.</param>
		/// <returns>The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.</returns>
		IDisposable Subscribe(IPairedObserver<TLeft, TRight> observer);
	}

	[ContractClassFor(typeof(IPairedObservable<,>))]
	internal abstract class IPairedObservableContract<TLeft, TRight> : IPairedObservable<TLeft, TRight>
	{
		public IDisposable Subscribe(IPairedObserver<TLeft, TRight> observer)
		{
			Contract.Requires(observer != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);
			return null;
		}

		public IDisposable Subscribe(IObserver<Either<TLeft, TRight>> observer)
		{
			return null;
		}
	}
}