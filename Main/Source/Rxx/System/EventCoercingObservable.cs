using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System
{
	internal sealed class EventCoercingObservable<TSource, TTarget> : CoercingObservable<IEvent<TSource>, IEvent<TTarget>>
	{
		#region Constructors
		public EventCoercingObservable(IObservable<IEvent<TSource>> source)
			: base(source)
		{
			Contract.Requires(source != null);
		}
		#endregion

		#region Methods
		protected override CoercingObserver<IEvent<TSource>, IEvent<TTarget>> CreateObserver(IObserver<IEvent<TTarget>> observer)
		{
			return new EventCoercingObserver<TSource, TTarget>(observer);
		}
		#endregion
	}
}
