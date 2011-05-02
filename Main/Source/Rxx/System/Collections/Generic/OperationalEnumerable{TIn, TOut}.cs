using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Collections.Generic
{
	/// <summary>
	/// Represents an enumerable that uses its unary and binary operator overloads as query operators.
	/// </summary>
	/// <typeparam name="TIn">Input type.</typeparam>
	/// <typeparam name="TOut">Output type.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "This class is not intended to be consumed publicly.  It's public only so that the compiler can resolve operator overloads.")]
	public class OperationalEnumerable<TIn, TOut> : IEnumerable<TIn>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IEnumerable<TIn> source;
		private readonly Func<IEnumerable<TOut>, OperationalEnumerable<TOut>> resultSelector;
		private readonly Func<IEnumerable<TIn>, IEnumerable<TIn>, Func<TIn, TIn, TOut>, IEnumerable<TOut>> binaryOperation;
		private readonly Func<TIn, TIn, TOut> add, subtract, multiply, divide;
		private readonly Func<TIn, TOut> positive, negative;
		#endregion

		#region Constructors
		internal OperationalEnumerable(
			IEnumerable<TIn> source,
			Func<IEnumerable<TIn>, IEnumerable<TIn>, Func<TIn, TIn, TOut>, IEnumerable<TOut>> binaryOperation,
			Func<TIn, TIn, TOut> add,
			Func<TIn, TIn, TOut> subtract,
			Func<TIn, TIn, TOut> multiply,
			Func<TIn, TIn, TOut> divide,
			Func<TIn, TOut> positive,
			Func<TIn, TOut> negative,
			Func<IEnumerable<TOut>, OperationalEnumerable<TOut>> resultSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(resultSelector != null);

			this.source = source;
			this.resultSelector = resultSelector;

			this.binaryOperation = binaryOperation ?? DefaultBinaryOperation;
			this.add = add ?? UnsupportedBinaryOperation;
			this.subtract = subtract ?? UnsupportedBinaryOperation;
			this.multiply = multiply ?? UnsupportedBinaryOperation;
			this.divide = divide ?? UnsupportedBinaryOperation;
			this.positive = positive ?? UnsupportedUnaryOperation;
			this.negative = negative ?? UnsupportedUnaryOperation;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(source != null);
			Contract.Invariant(resultSelector != null);
			Contract.Invariant(binaryOperation != null);
			Contract.Invariant(add != null);
			Contract.Invariant(subtract != null);
			Contract.Invariant(multiply != null);
			Contract.Invariant(divide != null);
			Contract.Invariant(positive != null);
			Contract.Invariant(negative != null);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<TIn> GetEnumerator()
		{
			return source.GetEnumerator();
		}

		IEnumerator Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[ContractVerification(false)]
		private static IEnumerable<TOut> DefaultBinaryOperation(IEnumerable<TIn> first, IEnumerable<TIn> second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<IEnumerable<TOut>>() != null);

			return from left in first.Select((value, index) => new { value, index })
						 join right in second.Select((value, index) => new { value, index })
						 on left.index equals right.index
						 select operation(left.value, right.value);
		}

		private static TOut UnsupportedBinaryOperation(TIn first, TIn second)
		{
			throw new NotSupportedException();
		}

		private static TOut UnsupportedUnaryOperation(TIn value)
		{
			throw new NotSupportedException();
		}

		private OperationalEnumerable<TOut> BinaryOperation(IEnumerable<TIn> second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(second != null);
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			var result = resultSelector(binaryOperation(this, second, operation));

			Contract.Assume(result != null);

			return result;
		}

		private OperationalEnumerable<TOut> BinaryOperation(TIn second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			var result = resultSelector(this.Select(first => operation(first, second)));

			Contract.Assume(result != null);

			return result;
		}

		private OperationalEnumerable<TOut> UnaryOperation(Func<TIn, TOut> operation)
		{
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			var result = resultSelector(this.Select(operation));

			Contract.Assume(result != null);

			return result;
		}
		#endregion

		#region Binary Operators
		/// <summary>
		/// Creates a new operational enumerable that adds the values in the specified enumerables 
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first enumerable.</param>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		/// <summary>
		/// Creates a new operational enumerable that adds the values in this enumerable to the values in the specified enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Add(IEnumerable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(second, add);
		}

		/// <summary>
		/// Creates a new operational enumerable that adds the values in the specified enumerable to the specified value
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The enumerable.</param>
		/// <param name="second">A value that is added to each value in the <paramref name="first"/> enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		/// <summary>
		/// Creates a new operational enumerable that adds the values in this enumerable to the specified value.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is added to each value in this enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Add(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(value, add);
		}

		/// <summary>
		/// Creates a new operational enumerable that subtracts the values in the specified enumerables 
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first enumerable.</param>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		/// <summary>
		/// Creates a new operational enumerable that subtracts the values in the specified enumerable from the values in this enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Subtract(IEnumerable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(second, subtract);
		}

		/// <summary>
		/// Creates a new operational enumerable that subtracts the specified value from the values in the specified enumerable
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The enumerable.</param>
		/// <param name="second">A value that is subtracted from each value in the <paramref name="first"/> enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		/// <summary>
		/// Creates a new operational enumerable that subtracts the specified value from the values in this enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is subtracted from each value in this enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Subtract(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(value, subtract);
		}

		/// <summary>
		/// Creates a new operational enumerable that multiplies the values in the specified enumerables 
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first enumerable.</param>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator *(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		/// <summary>
		/// Creates a new operational enumerable that multiplies the values in this enumerable by the values in the specified enumerable.
		/// </summary>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Multiply(IEnumerable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(second, multiply);
		}

		/// <summary>
		/// Creates a new operational enumerable that multiplies the values in the specified enumerable by the specified value
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The enumerable.</param>
		/// <param name="second">A value that is multiplied against each value in the <paramref name="first"/> enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator *(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		/// <summary>
		/// Creates a new operational enumerable that multiplies the values in this enumerable by the specified <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is multiplied against each value in this enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Multiply(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(value, multiply);
		}

		/// <summary>
		/// Creates a new operational enumerable that divides the values in the specified enumerables 
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first enumerable.</param>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator /(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		/// <summary>
		/// Creates a new operational enumerable that divides the values in this enumerable by the values in the specified enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Divide(IEnumerable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(second, divide);
		}

		/// <summary>
		/// Creates a new operational enumerable that divides the values in the specified enumerable by the specified value
		/// based on the binary operation logic of the <paramref name="first"/> enumerable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The enumerable.</param>
		/// <param name="second">A value that divides each value in the <paramref name="first"/> enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator /(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		/// <summary>
		/// Creates a new operational enumerable that divides the values in this enumerable by the specified <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that divides each value in this enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Divide(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return BinaryOperation(value, divide);
		}
		#endregion

		#region Unary Operators
		/// <summary>
		/// Creates a new operational enumerable that ensures the sign of the specified enumerable's values are positive.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> enumerable)
		{
			Contract.Requires(enumerable != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return enumerable.UnaryOperation(enumerable.positive);
		}

		/// <summary>
		/// Creates a new operational enumerable that ensures the sign of this enumerable's values are positive.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Plus()
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return UnaryOperation(positive);
		}

		/// <summary>
		/// Creates a new operational enumerable that ensures the sign of the specified enumerable's values are negative.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> enumerable)
		{
			Contract.Requires(enumerable != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return enumerable.UnaryOperation(enumerable.negative);
		}

		/// <summary>
		/// Creates a new operational enumerable that ensures the sign of this enumerable's values are negative.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalEnumerable.AsOperational(IEnumerable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <returns>An operational enumerable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalEnumerable<TOut> Negate()
		{
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return UnaryOperation(negative);
		}
		#endregion
	}
}