using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System
{
	public class OperationalObservable<TIn, TOut> : IObservable<TIn>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IObservable<TIn> source;
		private readonly Func<IObservable<TOut>, OperationalObservable<TOut>> resultSelector;
		private readonly Func<IObservable<TIn>, IObservable<TIn>, Func<TIn, TIn, TOut>, IObservable<TOut>> binaryOperation;
		private readonly Func<TIn, TIn, TOut> add, subtract, multiply, divide;
		private readonly Func<TIn, TOut> positive, negative;
		#endregion

		#region Constructors
		internal OperationalObservable(
			IObservable<TIn> source,
			Func<IObservable<TIn>, IObservable<TIn>, Func<TIn, TIn, TOut>, IObservable<TOut>> binaryOperation,
			Func<TIn, TIn, TOut> add,
			Func<TIn, TIn, TOut> subtract,
			Func<TIn, TIn, TOut> multiply,
			Func<TIn, TIn, TOut> divide,
			Func<TIn, TOut> positive,
			Func<TIn, TOut> negative,
			Func<IObservable<TOut>, OperationalObservable<TOut>> resultSelector)
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

		public IDisposable Subscribe(IObserver<TIn> observer)
		{
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			return source.Subscribe(observer);
		}

		[ContractVerification(false)]
		private static IObservable<TOut> DefaultBinaryOperation(IObservable<TIn> first, IObservable<TIn> second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<IObservable<TOut>>() != null);

			var observable = Observable.Join(first.And(second).Then(operation));

			Contract.Assume(observable != null);

			return observable;
		}

		private static TOut UnsupportedBinaryOperation(TIn first, TIn second)
		{
			throw new NotSupportedException();
		}

		private static TOut UnsupportedUnaryOperation(TIn value)
		{
			throw new NotSupportedException();
		}

		private OperationalObservable<TOut> BinaryOperation(IObservable<TIn> second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(second != null);
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			var result = resultSelector(binaryOperation(this, second, operation));

			Contract.Assume(result != null);

			return result;
		}

		private OperationalObservable<TOut> BinaryOperation(TIn second, Func<TIn, TIn, TOut> operation)
		{
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			var result = resultSelector(this.Select(first => operation(first, second)));

			Contract.Assume(result != null);

			return result;
		}

		private OperationalObservable<TOut> UnaryOperation(Func<TIn, TOut> operation)
		{
			Contract.Requires(operation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			var result = resultSelector(this.Select(operation));

			Contract.Assume(result != null);

			return result;
		}
		#endregion

		#region Binary Operators
		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator *(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator *(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator /(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator /(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}
		#endregion

		#region Unary Operators
		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return observable.UnaryOperation(observable.positive);
		}

		[ContractVerification(false)]
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Used as a combinator method in queries.")]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return observable.UnaryOperation(observable.negative);
		}
		#endregion
	}
}