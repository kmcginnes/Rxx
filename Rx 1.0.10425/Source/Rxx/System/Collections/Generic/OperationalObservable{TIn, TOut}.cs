using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Collections.Generic
{
	/// <summary>
	/// Represents an observable that uses its unary and binary operator overloads as observable query operators.
	/// </summary>
	/// <typeparam name="TIn">Input type.</typeparam>
	/// <typeparam name="TOut">Output type.</typeparam>
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

		/// <summary>
		/// Notifies the observable that an observer is to receive notifications.
		/// </summary>
		/// <param name="observer">The object that is to receive notifications.</param>
		/// <returns>The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.</returns>
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
		/// <summary>
		/// Creates a new operational observable that adds the values in the specified observables 
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first observable.</param>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		/// <summary>
		/// Creates a new operational observable that adds the values in this observable to the values in the specified observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Add(IObservable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(second, add);
		}

		/// <summary>
		/// Creates a new operational observable that adds the values in the specified observable to the specified value
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The observable.</param>
		/// <param name="second">A value that is added to each value in the <paramref name="first"/> observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.add);
		}

		/// <summary>
		/// Creates a new operational observable that adds the values in this observable to the specified <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is added to each value in this observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Add(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(value, add);
		}

		/// <summary>
		/// Creates a new operational observable that subtracts the values in the specified observables 
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first observable.</param>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		/// <summary>
		/// Creates a new operational observable that subtracts the values in the specified observable from the values in this observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Subtract(IObservable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(second, subtract);
		}

		/// <summary>
		/// Creates a new operational observable that subtracts the specified value from the values in the specified observable
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The observable.</param>
		/// <param name="second">A value that is subtracted from each value in the <paramref name="first"/> observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.subtract);
		}

		/// <summary>
		/// Creates a new operational observable that subtracts the specified value from the values in this observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is subtracted from each value in this observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Subtract(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(value, subtract);
		}

		/// <summary>
		/// Creates a new operational observable that multiplies the values in the specified observables 
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first observable.</param>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator *(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		/// <summary>
		/// Creates a new operational observable that multiplies the values in this observable with the values in the specified observable.
		/// </summary>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Multiply(IObservable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(second, multiply);
		}

		/// <summary>
		/// Creates a new operational observable that multiplies the values in the specified observable by the specified value
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The observable.</param>
		/// <param name="second">A value that is multiplied against each value in the <paramref name="first"/> observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator *(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.multiply);
		}

		/// <summary>
		/// Creates a new operational observable that multiplies the values in the specified observable by the specified <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that is multiplied against each value in this observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Multiply(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(value, multiply);
		}

		/// <summary>
		/// Creates a new operational observable that divides the values in the specified observables 
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The first observable.</param>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator /(OperationalObservable<TIn, TOut> first, IObservable<TIn> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		/// <summary>
		/// Creates a new operational observable that divides the values in this observable with the values in the specified observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="second">The second observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Divide(IObservable<TIn> second)
		{
			Contract.Requires(second != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(second, divide);
		}

		/// <summary>
		/// Creates a new operational observable that divides the values in the specified observable by the specified value
		/// based on the binary operation logic of the <paramref name="first"/> observable.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="first">The observable.</param>
		/// <param name="second">A value that divides each value in the <paramref name="first"/> observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator /(OperationalObservable<TIn, TOut> first, TIn second)
		{
			Contract.Requires(first != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return first.BinaryOperation(second, first.divide);
		}

		/// <summary>
		/// Creates a new operational observable that divides the values in this observable by the specified <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="value">A value that divides each value in this observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Divide(TIn value)
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return BinaryOperation(value, divide);
		}
		#endregion

		#region Unary Operators
		/// <summary>
		/// Creates a new operational observable that ensures the sign of the specified observable's values are positive.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="observable">The observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator +(OperationalObservable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return observable.UnaryOperation(observable.positive);
		}

		/// <summary>
		/// Creates a new operational observable that ensures the sign of this observable's values are positive.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Plus()
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return UnaryOperation(positive);
		}

		/// <summary>
		/// Creates a new operational observable that ensures the sign of the specified observable's values are negative.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <param name="observable">The observable.</param>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public static OperationalObservable<TOut> operator -(OperationalObservable<TIn, TOut> observable)
		{
			Contract.Requires(observable != null);
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return observable.UnaryOperation(observable.negative);
		}

		/// <summary>
		/// Creates a new operational observable that ensures the sign of this observable's values are negative.
		/// </summary>
		/// <remarks>
		/// <alert type="note">
		/// The actual behavior of this operator is determined by <see cref="System.Linq.OperationalObservable.AsOperational(IObservable{int})"/>.
		/// </alert>
		/// </remarks>
		/// <returns>An operational observable that generates the output of the operation.</returns>
		[ContractVerification(false)]
		public OperationalObservable<TOut> Negate()
		{
			Contract.Ensures(Contract.Result<OperationalObservable<TOut>>() != null);

			return UnaryOperation(negative);
		}
		#endregion
	}
}