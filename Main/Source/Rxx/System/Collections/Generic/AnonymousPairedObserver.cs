using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	internal sealed class AnonymousPairedObserver<TLeft, TRight> : IPairedObserver<TLeft, TRight>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IObserver<Either<TLeft, TRight>> observer;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="AnonymousPairedObserver" /> class.
		/// </summary>
		public AnonymousPairedObserver(IObserver<Either<TLeft, TRight>> observer)
		{
			Contract.Requires(observer != null);

			this.observer = observer;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(observer != null);
		}

		public void OnNextLeft(TLeft left)
		{
			observer.OnNext(new LeftValue<TLeft, TRight>(left));
		}

		public void OnNextRight(TRight right)
		{
			observer.OnNext(new RightValue<TLeft, TRight>(right));
		}

		public void OnNext(Either<TLeft, TRight> value)
		{
			observer.OnNext(value);
		}

		public void OnError(Exception error)
		{
			observer.OnError(error);
		}

		public void OnCompleted()
		{
			observer.OnCompleted();
		}
		#endregion
	}
}