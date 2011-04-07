using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class OperationalObservable
	{
		#region Public
		[CLSCompliant(false)]
		public static OperationalObservable<sbyte, int> AsOperational(
			this IObservable<sbyte> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<sbyte, int>>() != null);

			return AsOperationalInternal(source);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<sbyte, int> AsOperational(
			this IObservable<sbyte> source,
			Func<IObservable<sbyte>, IObservable<sbyte>, Func<sbyte, sbyte, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<sbyte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<sbyte, int> AsOperational(
			this IObservable<sbyte> source,
			Func<IObservable<sbyte>, IObservable<sbyte>, Func<sbyte, sbyte, int>, IObservable<int>> binaryOperation,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<sbyte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		public static OperationalObservable<byte, int> AsOperational(
			this IObservable<byte> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<byte, int>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<byte, int> AsOperational(
			this IObservable<byte> source,
			Func<IObservable<byte>, IObservable<byte>, Func<byte, byte, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<byte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<byte, int> AsOperational(
			this IObservable<byte> source,
			Func<IObservable<byte>, IObservable<byte>, Func<byte, byte, int>, IObservable<int>> binaryOperation,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<byte, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		public static OperationalObservable<char, int> AsOperational(
			this IObservable<char> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<char, int>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<char, int> AsOperational(
			this IObservable<char> source,
			Func<IObservable<char>, IObservable<char>, Func<char, char, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<char, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<char, int> AsOperational(
			this IObservable<char> source,
			Func<IObservable<char>, IObservable<char>, Func<char, char, int>, IObservable<int>> binaryOperation,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<char, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		public static OperationalObservable<short, int> AsOperational(
			this IObservable<short> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<short, int>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<short, int> AsOperational(
			this IObservable<short> source,
			Func<IObservable<short>, IObservable<short>, Func<short, short, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<short, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<short, int> AsOperational(
			this IObservable<short> source,
			Func<IObservable<short>, IObservable<short>, Func<short, short, int>, IObservable<int>> binaryOperation,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<short, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<ushort, int> AsOperational(
			this IObservable<ushort> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ushort, int>>() != null);

			return AsOperationalInternal(source);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<ushort, int> AsOperational(
			this IObservable<ushort> source,
			Func<IObservable<ushort>, IObservable<ushort>, Func<ushort, ushort, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ushort, int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<ushort, int> AsOperational(
			this IObservable<ushort> source,
			Func<IObservable<ushort>, IObservable<ushort>, Func<ushort, ushort, int>, IObservable<int>> binaryOperation,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Requires(resultBinaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ushort, int>>() != null);

			return AsOperationalInternal(source, binaryOperation, resultBinaryOperation);
		}

		public static OperationalObservable<int> AsOperational(
			this IObservable<int> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<int>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<int> AsOperational(
			this IObservable<int> source,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<int>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<uint> AsOperational(
			this IObservable<uint> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<uint>>() != null);

			return AsOperationalInternal(source);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<uint> AsOperational(
			this IObservable<uint> source,
			Func<IObservable<uint>, IObservable<uint>, Func<uint, uint, uint>, IObservable<uint>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<uint>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<long> AsOperational(
			this IObservable<long> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<long>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<long> AsOperational(
			this IObservable<long> source,
			Func<IObservable<long>, IObservable<long>, Func<long, long, long>, IObservable<long>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<long>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<ulong> AsOperational(
			this IObservable<ulong> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ulong>>() != null);

			return AsOperationalInternal(source);
		}

		[CLSCompliant(false)]
		public static OperationalObservable<ulong> AsOperational(
			this IObservable<ulong> source,
			Func<IObservable<ulong>, IObservable<ulong>, Func<ulong, ulong, ulong>, IObservable<ulong>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ulong>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<float> AsOperational(
			this IObservable<float> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<float>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<float> AsOperational(
			this IObservable<float> source,
			Func<IObservable<float>, IObservable<float>, Func<float, float, float>, IObservable<float>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<float>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<double> AsOperational(
			this IObservable<double> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<double>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<double> AsOperational(
			this IObservable<double> source,
			Func<IObservable<double>, IObservable<double>, Func<double, double, double>, IObservable<double>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<double>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}

		public static OperationalObservable<decimal> AsOperational(
			this IObservable<decimal> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<decimal>>() != null);

			return AsOperationalInternal(source);
		}

		public static OperationalObservable<decimal> AsOperational(
			this IObservable<decimal> source,
			Func<IObservable<decimal>, IObservable<decimal>, Func<decimal, decimal, decimal>, IObservable<decimal>> binaryOperation)
		{
			Contract.Requires(source != null);
			Contract.Requires(binaryOperation != null);
			Contract.Ensures(Contract.Result<OperationalObservable<decimal>>() != null);

			return AsOperationalInternal(source, binaryOperation);
		}
		#endregion

		#region Implementations
		private static OperationalObservable<sbyte, int> AsOperationalInternal(
			IObservable<sbyte> source,
			Func<IObservable<sbyte>, IObservable<sbyte>, Func<sbyte, sbyte, int>, IObservable<int>> binaryOperation = null,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<sbyte, int>>() != null);

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

		private static OperationalObservable<byte, int> AsOperationalInternal(
			IObservable<byte> source,
			Func<IObservable<byte>, IObservable<byte>, Func<byte, byte, int>, IObservable<int>> binaryOperation = null,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<byte, int>>() != null);

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

		private static OperationalObservable<char, int> AsOperationalInternal(
			IObservable<char> source,
			Func<IObservable<char>, IObservable<char>, Func<char, char, int>, IObservable<int>> binaryOperation = null,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<char, int>>() != null);

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

		private static OperationalObservable<short, int> AsOperationalInternal(
			IObservable<short> source,
			Func<IObservable<short>, IObservable<short>, Func<short, short, int>, IObservable<int>> binaryOperation = null,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<short, int>>() != null);

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

		private static OperationalObservable<ushort, int> AsOperationalInternal(
			IObservable<ushort> source,
			Func<IObservable<ushort>, IObservable<ushort>, Func<ushort, ushort, int>, IObservable<int>> binaryOperation = null,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> resultBinaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ushort, int>>() != null);

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

		private static OperationalObservable<int> AsOperationalInternal(
			IObservable<int> source,
			Func<IObservable<int>, IObservable<int>, Func<int, int, int>, IObservable<int>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<int>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalObservable<uint> AsOperationalInternal(
			IObservable<uint> source,
			Func<IObservable<uint>, IObservable<uint>, Func<uint, uint, uint>, IObservable<uint>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<uint>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value);
		}

		private static OperationalObservable<long> AsOperationalInternal(
			IObservable<long> source,
			Func<IObservable<long>, IObservable<long>, Func<long, long, long>, IObservable<long>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<long>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalObservable<ulong> AsOperationalInternal(
			IObservable<ulong> source,
			Func<IObservable<ulong>, IObservable<ulong>, Func<ulong, ulong, ulong>, IObservable<ulong>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<ulong>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value);
		}

		private static OperationalObservable<float> AsOperationalInternal(
			IObservable<float> source,
			Func<IObservable<float>, IObservable<float>, Func<float, float, float>, IObservable<float>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<float>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalObservable<double> AsOperationalInternal(
			IObservable<double> source,
			Func<IObservable<double>, IObservable<double>, Func<double, double, double>, IObservable<double>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<double>>() != null);

			return source.AsOperational(
				binaryOperation: binaryOperation,
				add: (first, second) => first + second,
				subtract: (first, second) => first - second,
				multiply: (first, second) => first * second,
				divide: (first, second) => first / second,
				positive: value => +value,
				negative: value => -value);
		}

		private static OperationalObservable<decimal> AsOperationalInternal(
			IObservable<decimal> source,
			Func<IObservable<decimal>, IObservable<decimal>, Func<decimal, decimal, decimal>, IObservable<decimal>> binaryOperation = null)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<OperationalObservable<decimal>>() != null);

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
