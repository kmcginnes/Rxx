namespace System
{
	public sealed class RightValue<TLeft, TRight> : Either<TLeft, TRight>
	{
		#region Public Properties
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
				throw new NotSupportedException();
			}
		}

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
		/// Constructs a new instance of the <see cref="RightValue" /> class.
		/// </summary>
		public RightValue(TRight value)
		{
			this.value = value;
		}
		#endregion

		#region Methods
		public override void Switch(Action<TLeft> left, Action<TRight> right)
		{
			right(value);
		}

		public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
		{
			return right(value);
		}
		#endregion
	}
}
