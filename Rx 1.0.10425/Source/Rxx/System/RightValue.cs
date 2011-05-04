﻿namespace System
{
	/// <summary>
	/// Holds the value of the right side of an <see cref="Either{TLeft,TRight}"/>.
	/// </summary>
	/// <typeparam name="TLeft">Type of the left side.</typeparam>
	/// <typeparam name="TRight">Type of the right side.</typeparam>
	public sealed class RightValue<TLeft, TRight> : Either<TLeft, TRight>
	{
		#region Public Properties
		/// <summary>
		/// Gets whether the object holds the left value or the right value.
		/// </summary>
		/// <value>Always returns <see langword="false" />.</value>
		public override bool IsLeft
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the left value when <see cref="IsLeft"/> is <see langword="true"/>.
		/// </summary>
		/// <value>Always throws <see cref="System.Diagnostics.Contracts.ContractException"/>.</value>
		/// <exception cref="System.Diagnostics.Contracts.ContractException">Always thrown by this property.</exception>
		public override TLeft Left
		{
			get
			{
				// precondition is inherited by Code Contracts
				return default(TLeft);
			}
		}

		/// <summary>
		/// Gets the right value when <see cref="IsLeft"/> is <see langword="false"/>.
		/// </summary>
		/// <value>Always returns the left value.</value>
		public override TRight Right
		{
			get
			{
				return value;
			}
		}
		#endregion

		#region Private / Protected
		private readonly TRight value;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="RightValue{TLeft,TRight}" /> class.
		/// </summary>
		public RightValue(TRight value)
		{
			this.value = value;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Invokes the specified <paramref name="right"/> action.
		/// </summary>
		/// <param name="left">Ignored.</param>
		/// <param name="right">The action to be invoked.</param>
		public override void Switch(Action<TLeft> left, Action<TRight> right)
		{
			right(value);
		}

		/// <summary>
		/// Invokes the specified <paramref name="right"/> function.
		/// </summary>
		/// <param name="left">Ignored.</param>
		/// <param name="right">The function to be invoked.</param>
		public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
		{
			return right(value);
		}
		#endregion
	}
}