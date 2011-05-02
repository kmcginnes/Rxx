namespace System.Linq
{
	/// <summary>
	/// Indicates whether an observed value being projected into an <see cref="System.Collections.Generic.IPairedObservable{TLeft,TRight}"/> is a
	/// <see cref="LeftValue{TLeft,TRight}"/>, <see cref="RightValue{TLeft,TRight}"/>, neither or both.
	/// </summary>
	public enum PairDirection
	{
		/// <summary>
		/// The value is excluded.
		/// </summary>
		Neither,

		/// <summary>
		/// The value is a <see cref="LeftValue{TLeft,TRight}"/>.
		/// </summary>
		Left,

		/// <summary>
		/// The value is a <see cref="RightValue{TLeft,TRight}"/>.
		/// </summary>
		Right,

		/// <summary>
		/// Ths value is both a <see cref="LeftValue{TLeft,TRight}"/> and a <see cref="RightValue{TLeft,TRight}"/>.
		/// </summary>
		Both
	}
}
