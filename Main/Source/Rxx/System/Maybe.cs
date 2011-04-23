using System.Diagnostics.Contracts;

namespace System
{
	public struct Maybe<T> : IEquatable<Maybe<T>>
	{
		#region Public Properties
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", 
			Justification = "It's simpler than constructing the static type; it also makes sense being that it's a struct.")]
		public static readonly Maybe<T> Empty = new Maybe<T>();

		public bool HasValue
		{
			get
			{
				return hasValue;
			}
		}

		public T Value
		{
			get
			{
				Contract.Requires(HasValue);

				return value;
			}
		}
		#endregion

		#region Private / Protected
		private readonly T value;
		private readonly bool hasValue;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="Maybe" /> class.
		/// </summary>
		public Maybe(T value)
		{
			this.value = value;
			hasValue = true;
		}
		#endregion

		#region Methods
		public static bool operator ==(Maybe<T> first, Maybe<T> second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(Maybe<T> first, Maybe<T> second)
		{
			return !first.Equals(second);
		}

		public override bool Equals(object obj)
		{
			return obj is Maybe<T>
					&& Equals((Maybe<T>) obj);
		}

		public bool Equals(Maybe<T> other)
		{
			return hasValue == other.hasValue
					&& (!hasValue || object.Equals(value, other.value));
		}

		public override int GetHashCode()
		{
			return hasValue
				? value == null ? 0 : value.GetHashCode()
				: -1;
		}

		public override string ToString()
		{
			return value == null ? string.Empty : value.ToString();
		}
		#endregion
	}
}
