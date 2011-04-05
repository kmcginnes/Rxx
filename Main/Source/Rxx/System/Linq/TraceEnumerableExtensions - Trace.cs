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
		public static IEnumerable<T> Trace<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>());

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(TraceDefaults.DefaultOnNext));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(TraceDefaults.GetFormatOnNext<T>(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source, Func<T, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetFormatOnError(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source, Func<Exception, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetMessageOnCompleted(message)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source, Func<string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(_ => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}
		#endregion

		#region System.Diagnostics.TraceSource
		public static IEnumerable<T> Trace<T>(this IEnumerable<T> source, TraceSource traceSource)
		{
			Contract.Requires(source != null);
			Contract.Requires(traceSource != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(traceSource));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, TraceDefaults.DefaultOnNext));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, TraceDefaults.GetFormatOnNext<T>(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnNext<T>(this IEnumerable<T> source, TraceSource trace, Func<T, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.GetFormatOnError(format)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnError<T>(this IEnumerable<T> source, TraceSource trace, Func<Exception, string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source, TraceSource trace, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.GetMessageOnCompleted(message)));

			Contract.Assume(enumerable != null);

			return enumerable;
		}

		public static IEnumerable<T> TraceOnCompleted<T>(this IEnumerable<T> source, TraceSource trace, Func<string> getMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(getMessage != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new TraceObserver<T>(trace, _ => null, getMessage));

			Contract.Assume(enumerable != null);

			return enumerable;
		}
		#endregion
	}
}
