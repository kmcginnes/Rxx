using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
	public static partial class TraceObservableExtensions
	{
		#region System.Diagnostics.Trace
		public static IObservable<T> TraceIdentity<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>());

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentity<T>(this IObservable<T> source, string identity)
		{
			Contract.Requires(source != null);
			Contract.Requires(!string.IsNullOrWhiteSpace(identity));
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>()
				{
					Identity = identity
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(TraceDefaults.GetIdentityFormatOnNext<T>(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source, Func<string, T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.GetIdentityFormatOnError(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source, Func<string, Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, TraceDefaults.GetIdentityMessageOnCompleted(message)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source, Func<string, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>((a, b) => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}
		#endregion

		#region System.Diagnostics.TraceSource
		public static IObservable<T> TraceIdentity<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, TraceDefaults.GetIdentityFormatOnNext<T>(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnNext<T>(this IObservable<T> source, TraceSource trace, Func<string, T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.GetIdentityFormatOnError(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnError<T>(this IObservable<T> source, TraceSource trace, Func<string, Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source, TraceSource trace, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, TraceDefaults.GetIdentityMessageOnCompleted(message)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceIdentityOnCompleted<T>(this IObservable<T> source, TraceSource trace, Func<string, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new IdentifiedTraceObserver<T>(trace, (a, b) => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}
		#endregion
	}
}
