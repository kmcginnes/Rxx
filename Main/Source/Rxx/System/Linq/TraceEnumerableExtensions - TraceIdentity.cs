using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
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

		public static IEnumerable<T> TraceIdentity<T>(this IEnumerable<T> source, string identity)
		{
			Contract.Requires(source != null);
			Contract.Requires(!string.IsNullOrWhiteSpace(identity));
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>()
				{
					Identity = identity
				});

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

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, Func<string, T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(messageSelector));

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

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, Func<string, Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, messageSelector));

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

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, Func<string, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, messageSelector));

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

		public static IEnumerable<T> TraceIdentityOnNext<T>(this IEnumerable<T> source, TraceSource trace, Func<string, T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, messageSelector));

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

		public static IEnumerable<T> TraceIdentityOnError<T>(this IEnumerable<T> source, TraceSource trace, Func<string, Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, messageSelector));

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

		public static IEnumerable<T> TraceIdentityOnCompleted<T>(this IEnumerable<T> source, TraceSource trace, Func<string, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			var enumerable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, messageSelector));

			Contract.Assume(enumerable != null);

			return enumerable;
		}
		#endregion
	}
}
