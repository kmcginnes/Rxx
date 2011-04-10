using System.Diagnostics.Contracts;

namespace System
{
	[ContractClass(typeof(EitherContract<,>))]
	public abstract class Either<TLeft, TRight>
	{
		#region Public Properties
		public abstract bool IsLeft { get; }

		public abstract TLeft Left { get; }

		public abstract TRight Right { get; }
		#endregion

		#region Private / Protected
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="Either" /> class for derived classes.
		/// </summary>
		protected Either()
		{
		}
		#endregion

		#region Methods
		public abstract void Switch(Action<TLeft> left, Action<TRight> right);

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
				return default(TLeft);
			}
		}

		public override TRight Right
		{
			get
			{
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
