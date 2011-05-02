namespace System
{
	/// <summary>
	/// Holds the value of the left side of an <see cref="Either{TLeft,TRight}"/>.
	/// </summary>
	/// <typeparam name="TLeft">Type of the left side.</typeparam>
	/// <typeparam name="TRight">Type of the right side.</typeparam>
	public sealed class LeftValue<TLeft, TRight> : Either<TLeft, TRight>
	{
		#region Public Properties
		/// <summary>
		/// Gets whether the object holds the left value or the right value.
		/// </summary>
		/// <value>Always returns <see langword="true" />.</value>
		public override bool IsLeft
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets the left value when <see cref="IsLeft"/> is <see langword="true"/>.
		/// </summary>
		/// <value>Always returns the left value.</value>
		public override TLeft Left
		{
			get
			{
				return value;
			}
		}

		/// <summary>
		/// Gets the right value when <see cref="IsLeft"/> is <see langword="false"/>.
		/// </summary>
		/// <value>Always throws <see cref="System.Diagnostics.Contracts.ContractException"/>.</value>
		/// <exception cref="System.Diagnostics.Contracts.ContractException">Always thrown by this property.</exception>
		public override TRight Right
		{
			get
			{
				// precondition is inherited by Code Contracts
				return default(TRight);
			}
		}
		#endregion

		#region Private / Protected
		private readonly TLeft value;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="LeftValue{TLeft,TRight}" /> class.
		/// </summary>
		public LeftValue(TLeft value)
		{
			this.value = value;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Invokes the specified <paramref name="left"/> action.
		/// </summary>
		/// <param name="left">The action to be invoked.</param>
		/// <param name="right">Ignored.</param>
		public override void Switch(Action<TLeft> left, Action<TRight> right)
		{
			left(value);
		}

		/// <summary>
		/// Invokes the specified <paramref name="left"/> function.
		/// </summary>
		/// <param name="left">The function to be invoked.</param>
		/// <param name="right">Ignored.</param>
		public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
		{
			return left(value);
		}
		#endregion
	}
}
