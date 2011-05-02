using System.Diagnostics.Contracts;

namespace System
{
	/// <summary>
	/// Represents one of two possible values.
	/// </summary>
	/// <typeparam name="TLeft">Type of the left value.</typeparam>
	/// <typeparam name="TRight">Type of the right value.</typeparam>
	[ContractClass(typeof(EitherContract<,>))]
	public abstract class Either<TLeft, TRight>
	{
		#region Public Properties
		/// <summary>
		/// Gets whether the object holds the left value or the right value.
		/// </summary>
		/// <value><see langword="true" /> if the object holds the left value; otherwise, <see langword="false"/>.</value>
		public abstract bool IsLeft { get; }

		/// <summary>
		/// Gets the left value when <see cref="IsLeft"/> is <see langword="true"/>.
		/// </summary>
		/// <value>The left value when <see cref="IsLeft"/> is <see langword="true"/>.</value>
		/// <exception cref="System.Diagnostics.Contracts.ContractException">Thrown when <see cref="IsLeft"/> is <see langword="false" />.</exception>
		public abstract TLeft Left { get; }

		/// <summary>
		/// Gets the right value when <see cref="IsLeft"/> is <see langword="false"/>.
		/// </summary>
		/// <value>The right value when <see cref="IsLeft"/> is <see langword="false"/>.</value>
		/// <exception cref="System.Diagnostics.Contracts.ContractException">Thrown when <see cref="IsLeft"/> is <see langword="true" />.</exception>
		public abstract TRight Right { get; }
		#endregion

		#region Private / Protected
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="Either{TLeft,TRight}" /> class for derived classes.
		/// </summary>
		protected Either()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Invokes the specified <paramref name="left"/> or <paramref name="right"/> action depending upon 
		/// the value of <see cref="IsLeft"/>.
		/// </summary>
		/// <param name="left">The action to be invoked when <see cref="IsLeft"/> is <see langword="true" />.</param>
		/// <param name="right">The action to be invoked when <see cref="IsLeft"/> is <see langword="false" />.</param>
		public abstract void Switch(Action<TLeft> left, Action<TRight> right);

		/// <summary>
		/// Invokes the specified <paramref name="left"/> or <paramref name="right"/> function depending upon 
		/// the value of <see cref="IsLeft"/>.
		/// </summary>
		/// <param name="left">The function to be invoked when <see cref="IsLeft"/> is <see langword="true" />.</param>
		/// <param name="right">The function to be invoked when <see cref="IsLeft"/> is <see langword="false" />.</param>
		public abstract TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right);
		#endregion
	}

	[ContractClassFor(typeof(Either<,>))]
	internal abstract class EitherContract<TLeft, TRight> : Either<TLeft, TRight>
	{
		public override bool IsLeft
		{
			get
			{
				return false;
			}
		}

		public override TLeft Left
		{
			get
			{
				Contract.Requires(IsLeft);

				return default(TLeft);
			}
		}

		public override TRight Right
		{
			get
			{
				Contract.Requires(!IsLeft);

				return default(TRight);
			}
		}

		public override void Switch(Action<TLeft> left, Action<TRight> right)
		{
			Contract.Requires(left != null);
			Contract.Requires(right != null);
		}

		public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
		{
			Contract.Requires(left != null);
			Contract.Requires(right != null);
			return default(TResult);
		}
	}
}
