using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class OperationalEnumerable
	{
		#region Public
		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="SByte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<sbyte, int> AsOperational(
			this IEnumerable<sbyte> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<sbyte, int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="SByte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<sbyte, int> AsOperational(
			this IEnumerable<sbyte> source,
			Func<IEnumerable<sbyte>, IEnumerable<sbyte>, Func<sbyte, sbyte, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<sbyte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="SByte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="resultBinaryOperation">The join behavior for binary operations on the resulting <see cref="OperationalEnumerable{TIn,TOut}"/>.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<sbyte, int> AsOperational(
			this IEnumerable<sbyte> source,
			Func<IEnumerable<sbyte>, IEnumerable<sbyte>, Func<sbyte, sbyte, int>, IEnumerable<int>> binaryOperation,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<sbyte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Byte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<byte, int> AsOperational(
			this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<byte, int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Byte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<byte, int> AsOperational(
			this IEnumerable<byte> source,
			Func<IEnumerable<byte>, IEnumerable<byte>, Func<byte, byte, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<byte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Byte"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="resultBinaryOperation">The join behavior for binary operations on the resulting <see cref="OperationalEnumerable{TIn,TOut}"/>.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<byte, int> AsOperational(
			this IEnumerable<byte> source,
			Func<IEnumerable<byte>, IEnumerable<byte>, Func<byte, byte, int>, IEnumerable<int>> binaryOperation,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<byte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Char"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<char, int> AsOperational(
			this IEnumerable<char> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<char, int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Char"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<char, int> AsOperational(
			this IEnumerable<char> source,
			Func<IEnumerable<char>, IEnumerable<char>, Func<char, char, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<char, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Char"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="resultBinaryOperation">The join behavior for binary operations on the resulting <see cref="OperationalEnumerable{TIn,TOut}"/>.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<char, int> AsOperational(
			this IEnumerable<char> source,
			Func<IEnumerable<char>, IEnumerable<char>, Func<char, char, int>, IEnumerable<int>> binaryOperation,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<char, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<short, int> AsOperational(
			this IEnumerable<short> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<short, int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<short, int> AsOperational(
			this IEnumerable<short> source,
			Func<IEnumerable<short>, IEnumerable<short>, Func<short, short, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<short, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="resultBinaryOperation">The join behavior for binary operations on the resulting <see cref="OperationalEnumerable{TIn,TOut}"/>.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<short, int> AsOperational(
			this IEnumerable<short> source,
			Func<IEnumerable<short>, IEnumerable<short>, Func<short, short, int>, IEnumerable<int>> binaryOperation,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<short, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<ushort, int> AsOperational(
			this IEnumerable<ushort> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ushort, int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<ushort, int> AsOperational(
			this IEnumerable<ushort> source,
			Func<IEnumerable<ushort>, IEnumerable<ushort>, Func<ushort, ushort, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ushort, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt16"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <param name="resultBinaryOperation">The join behavior for binary operations on the resulting <see cref="OperationalEnumerable{TIn,TOut}"/>.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<ushort, int> AsOperational(
			this IEnumerable<ushort> source,
			Func<IEnumerable<ushort>, IEnumerable<ushort>, Func<ushort, ushort, int>, IEnumerable<int>> binaryOperation,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ushort, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int32"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<int> AsOperational(
			this IEnumerable<int> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<int>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int32"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<int> AsOperational(
			this IEnumerable<int> source,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt32"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<uint> AsOperational(
			this IEnumerable<uint> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<uint>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt32"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<uint> AsOperational(
			this IEnumerable<uint> source,
			Func<IEnumerable<uint>, IEnumerable<uint>, Func<uint, uint, uint>, IEnumerable<uint>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<uint>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int64"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<long> AsOperational(
			this IEnumerable<long> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<long>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Int64"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<long> AsOperational(
			this IEnumerable<long> source,
			Func<IEnumerable<long>, IEnumerable<long>, Func<long, long, long>, IEnumerable<long>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<long>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt64"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<ulong> AsOperational(
			this IEnumerable<ulong> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ulong>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="UInt64"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		[CLSCompliant(false)]
		public static OperationalEnumerable<ulong> AsOperational(
			this IEnumerable<ulong> source,
			Func<IEnumerable<ulong>, IEnumerable<ulong>, Func<ulong, ulong, ulong>, IEnumerable<ulong>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ulong>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Single"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<float> AsOperational(
			this IEnumerable<float> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<float>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Single"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<float> AsOperational(
			this IEnumerable<float> source,
			Func<IEnumerable<float>, IEnumerable<float>, Func<float, float, float>, IEnumerable<float>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<float>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Double"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<double> AsOperational(
			this IEnumerable<double> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<double>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Double"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<double> AsOperational(
			this IEnumerable<double> source,
			Func<IEnumerable<double>, IEnumerable<double>, Func<double, double, double>, IEnumerable<double>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<double>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Decimal"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<decimal> AsOperational(
			this IEnumerable<decimal> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<decimal>>() != null);

			return AsOperationalInternal(source);
		}

		/// <summary>
		/// Creates a standard <see cref="OperationalEnumerable{TIn,TOut}"/> for the specified <see cref="Decimal"/> <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The enumerable to be converted.</param>
		/// <param name="binaryOperation">The join behavior for binary operations.</param>
		/// <returns>An <see cref="OperationalEnumerable{TIn,TOut}"/> that applies the specified operations to the specified <paramref name="source"/> 
		/// when combined with another enumerable.</returns>
		public static OperationalEnumerable<decimal> AsOperational(
			this IEnumerable<decimal> source,
			Func<IEnumerable<decimal>, IEnumerable<decimal>, Func<decimal, decimal, decimal>, IEnumerable<decimal>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<decimal>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}
		#endregion

		#region Implementations
		private static OperationalEnumerable<sbyte, int> AsOperationalInternal(
			IEnumerable<sbyte> source,
			Func<IEnumerable<sbyte>, IEnumerable<sbyte>, Func<sbyte, sbyte, int>, IEnumerable<int>> binaryOperation = null,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<sbyte, int>>() != null);

			return source.AsOperational(
				result => AsOperationalInternal(result, resultBinaryOperation),
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<byte, int> AsOperationalInternal(
			IEnumerable<byte> source,
			Func<IEnumerable<byte>, IEnumerable<byte>, Func<byte, byte, int>, IEnumerable<int>> binaryOperation = null,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<byte, int>>() != null);

			return source.AsOperational(
				result => AsOperationalInternal(result, resultBinaryOperation),
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<char, int> AsOperationalInternal(
			IEnumerable<char> source,
			Func<IEnumerable<char>, IEnumerable<char>, Func<char, char, int>, IEnumerable<int>> binaryOperation = null,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<char, int>>() != null);

			return source.AsOperational(
				result => AsOperationalInternal(result, resultBinaryOperation),
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<short, int> AsOperationalInternal(
			IEnumerable<short> source,
			Func<IEnumerable<short>, IEnumerable<short>, Func<short, short, int>, IEnumerable<int>> binaryOperation = null,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<short, int>>() != null);

			return source.AsOperational(
				result => AsOperationalInternal(result, resultBinaryOperation),
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<ushort, int> AsOperationalInternal(
			IEnumerable<ushort> source,
			Func<IEnumerable<ushort>, IEnumerable<ushort>, Func<ushort, ushort, int>, IEnumerable<int>> binaryOperation = null,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ushort, int>>() != null);

			return source.AsOperational(
				result => AsOperationalInternal(result, resultBinaryOperation),
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<int> AsOperationalInternal(
			IEnumerable<int> source,
			Func<IEnumerable<int>, IEnumerable<int>, Func<int, int, int>, IEnumerable<int>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<int>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<uint> AsOperationalInternal(
			IEnumerable<uint> source,
			Func<IEnumerable<uint>, IEnumerable<uint>, Func<uint, uint, uint>, IEnumerable<uint>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<uint>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value);
		}

		private static OperationalEnumerable<long> AsOperationalInternal(
			IEnumerable<long> source,
			Func<IEnumerable<long>, IEnumerable<long>, Func<long, long, long>, IEnumerable<long>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<long>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<ulong> AsOperationalInternal(
			IEnumerable<ulong> source,
			Func<IEnumerable<ulong>, IEnumerable<ulong>, Func<ulong, ulong, ulong>, IEnumerable<ulong>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<ulong>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value);
		}

		private static OperationalEnumerable<float> AsOperationalInternal(
			IEnumerable<float> source,
			Func<IEnumerable<float>, IEnumerable<float>, Func<float, float, float>, IEnumerable<float>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<float>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<double> AsOperationalInternal(
			IEnumerable<double> source,
			Func<IEnumerable<double>, IEnumerable<double>, Func<double, double, double>, IEnumerable<double>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<double>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalEnumerable<decimal> AsOperationalInternal(
			IEnumerable<decimal> source,
			Func<IEnumerable<decimal>, IEnumerable<decimal>, Func<decimal, decimal, decimal>, IEnumerable<decimal>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalEnumerable<decimal>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}
		#endregion
	}
}
