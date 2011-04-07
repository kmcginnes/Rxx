using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class OperationalEnumerable
	{
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
			Justification = "In this case, optional parameters are more flexible than defining only a subset of all possible combinations.")]
		public static OperationalEnumerable<T> AsOperational<T>(
			this IEnumerable<T> source,
			Func<IEnumerable<T>, IEnumerable<T>, Func<T, T, T>, IEnumerable<T>> binaryOperation = null,
			Func<T, T, T> add = null,
			Func<T, T, T> subtract = null,
			Func<T, T, T> multiply = null,
			Func<T, T, T> divide = null,
			Func<T, T> positive = null,
			Func<T, T> negative = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<T>>() != null);

			return new OperationalEnumerable<T>(
				source,
				binaryOperation,
				add,
				subtract,
				multiply,
				divide,
				positive,
				negative);
		}

		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
			Justification = "In this case, optional parameters are more flexible than defining only a subset of all possible combinations.")]
		public static OperationalEnumerable<TIn, TOut> AsOperational<TIn, TOut>(
			this IEnumerable<TIn> source,
			Func<IEnumerable<TOut>, OperationalEnumerable<TOut>> resultSelector,
			Func<IEnumerable<TIn>, IEnumerable<TIn>, Func<TIn, TIn, TOut>, IEnumerable<TOut>> binaryOperation = null,
			Func<TIn, TIn, TOut> add = null,
			Func<TIn, TIn, TOut> subtract = null,
			Func<TIn, TIn, TOut> multiply = null,
			Func<TIn, TIn, TOut> divide = null,
			Func<TIn, TOut> positive = null,
			Func<TIn, TOut> negative = null)
		{
			Contract.Requires(source != null);
			Contract.Requires(resultSelector != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TIn, TOut>>() != null);

			return new OperationalEnumerable<TIn, TOut>(
				source,
				binaryOperation,
				add,
				subtract,
				multiply,
				divide,
				positive,
				negative,
				resultSelector);
		}
	}
}
