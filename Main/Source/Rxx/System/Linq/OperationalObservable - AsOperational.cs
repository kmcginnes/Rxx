using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class OperationalObservable
	{
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
			Justification = "In this case, optional parameters are more flexible than defining only a subset of all possible combinations.")]
		public static OperationalObservable<T> AsOperational<T>(
			this IObservable<T> source,
			Func<IObservable<T>, IObservable<T>, Func<T, T, T>, IObservable<T>> binaryOperation = null,
			Func<T, T, T> add = null,
			Func<T, T, T> subtract = null,
			Func<T, T, T> multiply = null,
			Func<T, T, T> divide = null,
			Func<T, T> positive = null,
			Func<T, T> negative = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<T>>() != null);

			return new OperationalObservable<T>(
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
		public static OperationalObservable<TIn, TOut> AsOperational<TIn, TOut>(
			this IObservable<TIn> source,
			Func<IObservable<TOut>, OperationalObservable<TOut>> resultSelector,
			Func<IObservable<TIn>, IObservable<TIn>, Func<TIn, TIn, TOut>, IObservable<TOut>> binaryOperation = null,
			Func<TIn, TIn, TOut> add = null,
			Func<TIn, TIn, TOut> subtract = null,
			Func<TIn, TIn, TOut> multiply = null,
			Func<TIn, TIn, TOut> divide = null,
			Func<TIn, TOut> positive = null,
			Func<TIn, TOut> negative = null)
		{
			Contract.Requires(source != null);
			Contract.Requires(resultSelector != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TIn, TOut>>() != null);

			return new OperationalObservable<TIn, TOut>(
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
