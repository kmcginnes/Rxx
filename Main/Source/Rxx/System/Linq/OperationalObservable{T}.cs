using System.Diagnostics.Contracts;

namespace System.Linq
{
	public sealed class OperationalObservable<T> : OperationalObservable<T, T>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		#endregion

		#region Constructors
		internal OperationalObservable(
			IObservable<T> source,
			Func<IObservable<T>, IObservable<T>, Func<T, T, T>, IObservable<T>> binaryOperation,
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
					result => new OperationalObservable<T>(
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