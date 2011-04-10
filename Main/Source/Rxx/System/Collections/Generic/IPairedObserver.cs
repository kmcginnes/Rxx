namespace System.Collections.Generic
{
	public interface IPairedObserver<TLeft, TRight> : IObserver<Either<TLeft, TRight>>
	{
		void OnNextLeft(TLeft left);

		void OnNextRight(TRight right);
	}
}
