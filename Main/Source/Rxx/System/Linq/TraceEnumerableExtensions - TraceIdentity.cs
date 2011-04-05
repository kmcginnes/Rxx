using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
	/* This class is named TraceEnumerableExtensions instead of EnumerableExtensions2 because the Trace<T> methods cause the C# compiler
	 * to complain when using static methods on System.Diagnostics.Trace; i.e., it must be fully qualified when used in extension methods.
	 */

	public static partial class TraceEnumerableExtensions
	{
		#region System.Diagnostics.Trace
		public static IEnumerable<T> TraceIdentity<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>());

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(TraceDefaults.DefaultOnNext));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(TraceDefaults.GetIdentityFormatOnNext<T>(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, Func<int, T, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.DefaultOnError));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.GetIdentityFormatOnError(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, Func<int, Exception, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.GetIdentityMessageOnCompleted(message)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, Func<int, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}
		#endregion

		#region System.Diagnostics.TraceSource
		public static IEnumerable<T> TraceIdentity<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, TraceDefaults.DefaultOnNext));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, TraceDefaults.GetIdentityFormatOnNext<T>(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, TraceSource trace, Func<int, T, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.DefaultOnError));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.GetIdentityFormatOnError(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, TraceSource trace, Func<int, Exception, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, TraceSource trace, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.GetIdentityMessageOnCompleted(message)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, TraceSource trace, Func<int, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}
		#endregion
	}
}
