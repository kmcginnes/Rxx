using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System
{
	internal sealed class EventCoercingObserver<TSource, TTarget> : CoercingObserver<IEvent<TSource>, IEvent<TTarget>>
	{
		#region Constructors
		public EventCoercingObserver(IObserver<IEvent<TTarget>> target)
			: base(target)
		{
			Contract.Requires(target != null);
		}
		#endregion

		#region Methods
		protected override IEvent<TTarget> Convert(IEvent<TSource> value)
		{
			Contract.Assume(value != null);

			return new CoercingEvent<TSource, TTarget>(value);
		}
		#endregion
	}
}
