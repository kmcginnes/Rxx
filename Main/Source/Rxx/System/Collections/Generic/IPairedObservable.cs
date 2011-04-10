using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	[ContractClass(typeof(IPairedObservableContract<,>))]
	public interface IPairedObservable<TLeft, TRight> : IObservable<Either<TLeft, TRight>>
	{
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