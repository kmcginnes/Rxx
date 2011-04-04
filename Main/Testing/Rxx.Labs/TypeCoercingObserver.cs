using System;
using System.Diagnostics.Contracts;

namespace Rxx.Labs
{
	internal sealed class TypeCoercingObserver<TIn, TOut> : IObserver<TIn>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IObserver<TOut> observer;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="TypeCoercingObserver" /> class.
		/// </summary>
		public TypeCoercingObserver(IObserver<TOut> observer)
		{
			Contract.Assume(observer != null);

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
		#endregion

		#region IObserver<TIn> Members
		public void OnNext(TIn value)
		{
			var obj = (object) value;

			if (obj == null)
				observer.OnNext(default(TOut));
			else
				observer.OnNext((TOut) obj);
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