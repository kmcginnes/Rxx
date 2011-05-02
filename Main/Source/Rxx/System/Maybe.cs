using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System
{
	/// <summary>
	/// Represents an instance of an object or a missing instance of an object.
	/// </summary>
	/// <typeparam name="T">Type of object.</typeparam>
	public struct Maybe<T> : IEquatable<Maybe<T>>
	{
		#region Public Properties
		/// <summary>
		/// Indicates a missing instance.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
			Justification = "It's simpler than constructing the static type; it also makes sense being that it's a struct.")]
		public static readonly Maybe<T> Empty = new Maybe<T>();

		/// <summary>
		/// Gets whether the <see cref="Value"/> is available.
		/// </summary>
		/// <value><see langword="true"/> if <see cref="Value"/> is available; otherwise, <see langword="false" />.</value>
		public bool HasValue
		{
			get
			{
				return hasValue;
			}
		}

		/// <summary>
		/// Gets the value when <see cref="HasValue"/> is <see langword="true" />.
		/// </summary>
		/// <exception cref="System.Diagnostics.Contracts.ContractException"><see cref="HasValue"/> is <see langword="false"/>.</exception>
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
		/// Constructs a new instance of the <see cref="Maybe{T}" /> class with the specified availble <paramref name="value"/>.
		/// </summary>
		/// <remarks>
		/// Constructing a <see cref="Maybe{T}"/> instance with this constructor always sets <see cref="HasValue"/> to <see langword="true" />.
		/// </remarks>
		/// <param name="value">The value assigned to the <see cref="Value"/> property.</param>
		public Maybe(T value)
		{
			this.value = value;
			hasValue = true;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines the equality of two <see cref="Maybe{T}"/> values.
		/// </summary>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		/// <returns><see langword="true"/> if the <paramref name="first"/> value equals the <paramref name="second"/> value; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Maybe<T> first, Maybe<T> second)
		{
			return first.Equals(second);
		}

		/// <summary>
		/// Determines the inequality of two <see cref="Maybe{T}"/> values.
		/// </summary>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		/// <returns><see langword="true"/> if the <paramref name="first"/> value does not equal the <paramref name="second"/> value; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Maybe<T> first, Maybe<T> second)
		{
			return !first.Equals(second);
		}

		/// <summary>
		/// Determines the equality of this instance and the specified <paramref name="obj"/>.
		/// </summary>
		/// <param name="obj">The object that is compared to this instance.</param>
		/// <returns><see langword="true"/> if this instance equals the specified <paramref name="obj"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			return obj is Maybe<T>
					&& Equals((Maybe<T>) obj);
		}

		/// <summary>
		/// Determines the equality of this instance and the <paramref name="other"/> instance.
		/// </summary>
		/// <param name="other">The instance that is compared to this instance.</param>
		/// <returns><see langword="true"/> if this instance equals the <paramref name="other"/> instance; otherwise, <see langword="false" />.</returns>
		public bool Equals(Maybe<T> other)
		{
			return hasValue == other.hasValue
					&& (!hasValue || EqualityComparer<T>.Default.Equals(value, other.value));
		}

		/// <summary>
		/// Gets the hash code of this instance.
		/// </summary>
		/// <returns>-1 if <see cref="HasValue"/> is <see langword="false" /> and 0 if <see cref="Value"/> is <see langword="null"/>; otherwise, the hash code of <see cref="Value"/>.</returns>
		public override int GetHashCode()
		{
			return hasValue
				? value == null ? 0 : value.GetHashCode()
				: -1;
		}

		/// <summary>
		/// Returns the string representation of <see cref="Value"/>.
		/// </summary>
		/// <returns>String that represents this instance.</returns>
		public override string ToString()
		{
			return value == null ? string.Empty : value.ToString();
		}
		#endregion
	}
}
