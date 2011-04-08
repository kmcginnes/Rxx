using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System
{
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
		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator *(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator *(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator /(OperationalEnumerable<TIn, TOut> first, IEnumerable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator /(OperationalEnumerable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}
		#endregion

		#region Unary Operators
		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator +(OperationalEnumerable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return observable.UnaryOperation(observable.positive);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalEnumerable<TOut> operator -(OperationalEnumerable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<TOut>>() != null);

			return observable.UnaryOperation(observable.negative);
		}
		#endregion
	}
}