namespace System
{
	public sealed class LeftValue<TLeft, TRight> : Either<TLeft, TRight>
	{
		#region Public Properties
		public override bool IsLeft
		{
			get
			{
				return true;
			}
		}

		public override TLeft Left
		{
			get
			{
				return value;
			}
		}

		public override TRight Right
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		#endregion

		#region Private / Protected
		private readonly TLeft value;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="LeftValue" /> class.
		/// </summary>
		public LeftValue(TLeft value)
		{
			this.value = value;
		}
		#endregion

		#region Methods
		public override void Switch(Action<TLeft> left, Action<TRight> right)
		{
			left(value);
		}

		public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
		{
			return left(value);
		}
		#endregion
	}
}
