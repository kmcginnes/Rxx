using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	/// <summary>
	/// Provides extension methods that convert an <see cref="IEnumerable{T}"/> into an <see cref="OperationalEnumerable{TIn,TOut}"/>.
	/// </summary>
	public static partial class OperationalEnumerable
	{
		/// <summary>
		/// Creates an <see cref="OperationalEnumerable{T}"/> for the specified <paramref name="source"/> from the specified operators.
		/// </summary>
		/// <typeparam name="T">The type of objects to enumerate.</typeparam>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="add">The addition operator.</param>
		/// <param name="subtract">The subtraction operator.</param>
		/// <param name="multiply">The multiplication operator.</param>
		/// <param name="divide">The division operator.</param>
		/// <param name="positive">The plus operator.</param>
		/// <param name="negative">The negation operator.</param>
		/// <returns>An <see cref="OperationalEnumerable{T}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
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

		/// <summary>
		/// Creates an <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <paramref name="source"/> from the specified operators.
		/// </summary>
		/// <typeparam name="TIn">The type of input to enumerate.</typeparam>
		/// <typeparam name="TOut">The type of output that each operation generates.</typeparam>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="resultSelector">Projects the result sequence into an <see cref="OperationalEnumerable{T}"/>.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="add">The addition operator.</param>
		/// <param name="subtract">The subtraction operator.</param>
		/// <param name="multiply">The multiplication operator.</param>
		/// <param name="divide">The division operator.</param>
		/// <param name="positive">The plus operator.</param>
		/// <param name="negative">The negation operator.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
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
