using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	internal sealed class AnonymousPairedObservable<TLeft, TRight> : IPairedObservable<TLeft, TRight>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IObservable<Either<TLeft, TRight>> observable;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="AnonymousPairedObservable" /> class.
		/// </summary>
		public AnonymousPairedObservable(IObservable<Either<TLeft, TRight>> observable)
		{
			Contract.Requires(observable != null);

			this.observable = observable;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(observable != null);
		}

		public IDisposable Subscribe(IPairedObserver<TLeft, TRight> observer)
		{
			return observable.Subscribe(observer);
		}

		public IDisposable Subscribe(IObserver<Either<TLeft, TRight>> observer)
		{
			return observable.Subscribe(observer);
		}
		#endregion
	}
}
