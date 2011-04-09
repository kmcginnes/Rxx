using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	internal sealed class CoercingEvent<TSource, TTarget> : IEvent<TTarget>
	{
		#region Public Properties
		public TTarget EventArgs
		{
			[ContractVerification(false)]
			[SuppressMessage("Microsoft.StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
				Justification = "Double cast.")]
			get
			{
				return (TTarget) (object) source.EventArgs;
			}
		}

		public object Sender
		{
			get
			{
				return source.Sender;
			}
		}
		#endregion

		#region Private / Protected
		private readonly IEvent<TSource> source;
		#endregion

		#region Constructors
		public CoercingEvent(IEvent<TSource> source)
		{
			Contract.Requires(source != null);

			this.source = source;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(source != null);
		}
		#endregion
	}
}
