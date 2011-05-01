using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
	public static partial class TraceObservableExtensions
	{
		#region System.Diagnostics.Trace
		public static IObservable<T> Trace<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>());

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(TraceDefaults.GetFormatOnNext<T>(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, Func<T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetFormatOnError(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, Func<Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetMessageOnCompleted(message)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, Func<string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}
		#endregion

		#region System.Diagnostics.TraceSource
		public static IObservable<T> Trace<T>(this IObservable<T> source, TraceSource traceSource)
		{
			Contract.Requires(source != null);
			Contract.Requires(traceSource != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(traceSource));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, TraceDefaults.GetFormatOnNext<T>(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, TraceSource trace, Func<T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, TraceSource trace, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.GetFormatOnError(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, TraceSource trace, Func<Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, TraceSource trace, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.GetMessageOnCompleted(message)));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, TraceSource trace, Func<string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}
		#endregion
	}
}
