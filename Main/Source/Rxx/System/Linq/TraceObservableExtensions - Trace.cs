using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
	/// <summary>
	/// Provides extension methods that trace observables.
	/// </summary>
	public static partial class TraceObservableExtensions
	{
		#region System.Diagnostics.Trace
		/// <summary>
		/// Returns an observable that traces OnNext, OnError and OnCompleted calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <returns>An observable that traces all notifications.</returns>
		public static IObservable<T> Trace<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>());

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="format">The format in which values will be traced.  A single replacement token {0} is supported.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(TraceDefaults.GetFormatOnNext<T>(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for each notification.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, Func<T, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
		public static IObservable<T> TraceOnError<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <param name="format">The format in which the error will be traced.  A single replacement token {0} is supported.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, string format)
		{
			Contract.Requires(source != null);
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetFormatOnError(format)));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for the error.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, Func<Exception, string> messageSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(messageSelector != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, messageSelector));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <param name="message">The message to be traced for the completed notification.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, string message)
		{
			Contract.Requires(source != null);
			Contract.Requires(message != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(_ => null, TraceDefaults.GetMessageOnCompleted(message)));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for the completed notification.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
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
		/// <summary>
		/// Returns an observable that traces OnNext, OnError and OnCompleted calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="traceSource">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <returns>An observable that traces all notifications.</returns>
		public static IObservable<T> Trace<T>(this IObservable<T> source, TraceSource traceSource)
		{
			Contract.Requires(source != null);
			Contract.Requires(traceSource != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(traceSource));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
		public static IObservable<T> TraceOnNext<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, TraceDefaults.DefaultOnNext));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="format">The format in which values will be traced.  A single replacement token {0} is supported.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
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

		/// <summary>
		/// Returns an observable that traces OnNext calls from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which notifications will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for each notification.</param>
		/// <returns>An observable that traces OnNext notifications.</returns>
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

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
		public static IObservable<T> TraceOnError<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnError));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="format">The format in which the error will be traced.  A single replacement token {0} is supported.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
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

		/// <summary>
		/// Returns an observable that traces a call to OnError from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the error will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for the error.</param>
		/// <returns>An observable that traces a call to OnError.</returns>
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

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
		public static IObservable<T> TraceOnCompleted<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = source.Do(new TraceObserver<T>(trace, _ => null, TraceDefaults.DefaultOnCompleted));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="message">The message to be traced for the completed notification.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
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

		/// <summary>
		/// Returns an observable that traces a call to OnCompleted from the specified observable.
		/// </summary>
		/// <typeparam name="T">The object that provides notification information.</typeparam>
		/// <param name="source">The observable from which the completed notification will be traced.</param>
		/// <param name="trace">The <see cref="TraceSource"/> to be associated with the trace messages.</param>
		/// <param name="messageSelector">A function that returns the message to be traced for the completed notification.</param>
		/// <returns>An observable that traces a call to OnCompleted.</returns>
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
