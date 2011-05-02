using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	/// <summary>
	/// Provides extension methods that convert an <see cref="IObservable{T}"/> into an <see cref="OperationalObservable{TIn,TOut}"/>.
	/// </summary>
	public static partial class OperationalObservable
	{
		/// <summary>
		/// Creates an <see cref="OperationalObservable{T}"/> for the specified <paramref name="source"/> from the specified operators.
		/// </summary>
		/// <typeparam name="T">The type of objects to observe.</typeparam>
		/// <param name="source">The observable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="add">The addition operator.</param>
		/// <param name="subtract">The subtraction operator.</param>
		/// <param name="multiply">The multiplication operator.</param>
		/// <param name="divide">The division operator.</param>
		/// <param name="positive">The plus operator.</param>
		/// <param name="negative">The negation operator.</param>
		/// <returns>An <see cref="OperationalObservable{T}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another observable.</returns>
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

		/// <summary>
		/// Creates an <see cref="OperationalObservable{TIn,TOut}"/> for the specified <paramref name="source"/> from the specified operators.
		/// </summary>
		/// <typeparam name="TIn">The type of input to observe.</typeparam>
		/// <typeparam name="TOut">The type of output that each operation generates.</typeparam>
		/// <param name="source">The observable to be converted.</param>
		/// <param name="resultSelector">Projects the result sequence into an <see cref="OperationalObservable{T}"/>.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="add">The addition operator.</param>
		/// <param name="subtract">The subtraction operator.</param>
		/// <param name="multiply">The multiplication operator.</param>
		/// <param name="divide">The division operator.</param>
		/// <param name="positive">The plus operator.</param>
		/// <param name="negative">The negation operator.</param>
		/// <returns>An <see cref="OperationalObservable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another observable.</returns>
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
