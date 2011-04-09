using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "This class is not intended to be consumed publicly.  It's public only so that the compiler can resolve operator overloads.")]
	public sealed class OperationalEnumerable<T> : OperationalEnumerable<T, T>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		#endregion

		#region Constructors
		internal OperationalEnumerable(
			IEnumerable<T> source,
			Func<IEnumerable<T>, IEnumerable<T>, Func<T, T, T>, IEnumerable<T>> binaryOperation,
			Func<T, T, T> add,
			Func<T, T, T> subtract,
			Func<T, T, T> multiply,
			Func<T, T, T> divide,
			Func<T, T> positive,
			Func<T, T> negative)
			: base(
					source,
					binaryOperation,
					add,
					subtract,
					multiply,
					divide,
					positive,
					negative,
					result => new OperationalEnumerable<T>(
						result,
						binaryOperation,
						add,
						subtract,
						multiply,
						divide,
						positive,
						negative))
		{
			Contract.Requires(source != null);
		}
		#endregion

		#region Methods
		#endregion
	}
}