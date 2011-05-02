using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	/// <summary>
	/// Provides a set of <see langword="static"/> methods for query operations over observable sequences.
	/// </summary>
	public static partial class Observable2
	{
		[ContractVerification(false)]
		internal static object Coerce(this IObservable<object> source, Type targetElementType)
		{
			Contract.Requires(source != null);
			Contract.Requires(targetElementType != null);
			Contract.Ensures(Contract.Result<object>() != null);

			return Activator.CreateInstance(
				typeof(CoercingObservable<,>).MakeGenericType(typeof(object), targetElementType),
				source);
		}

		[ContractVerification(false)]
		internal static object Coerce(this IObservable<IEvent<object>> source, Type targetEventArgsType)
		{
			Contract.Requires(source != null);
			Contract.Requires(targetEventArgsType != null);
			Contract.Ensures(Contract.Result<object>() != null);

			return Activator.CreateInstance(
				typeof(EventCoercingObservable<,>).MakeGenericType(typeof(object), targetEventArgsType),
				source);
		}
	}
}
