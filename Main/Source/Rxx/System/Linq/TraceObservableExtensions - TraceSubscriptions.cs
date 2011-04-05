using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Disposables;
using System.Globalization;
using TraceSource = System.Diagnostics.TraceSource;

namespace System.Linq
{
	/* This class is named TraceObservableExtensions instead of ObservableExtensions2 because the Trace<T> methods cause the C# compiler
	 * to complain when using static methods on System.Diagnostics.Trace; i.e., it must be fully qualified when used in extension methods.
	 */

	public static partial class TraceObservableExtensions
	{
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultSubscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultDisposingSubscriptionMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultDisposedSubscriptionMessage)));

				System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultSubscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, string name)
		{
			Contract.Requires(source != null);
			Contract.Requires(name != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			return source.TraceSubscriptions(
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.SubscribingFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.SubscribedFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.DisposingSubscriptionFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.DisposedSubscriptionFormat, name));
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, string subscribingMessage, string subscribedMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(subscribingMessage != null);
			Contract.Requires(subscribedMessage != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				System.Diagnostics.Trace.TraceInformation(subscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultDisposingSubscriptionMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(Rxx.Properties.Text.DefaultDisposedSubscriptionMessage)));

				System.Diagnostics.Trace.TraceInformation(subscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, string subscribingMessage, string subscribedMessage, string disposingMessage, string disposedMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(subscribingMessage != null);
			Contract.Requires(subscribedMessage != null);
			Contract.Requires(disposingMessage != null);
			Contract.Requires(disposedMessage != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				System.Diagnostics.Trace.TraceInformation(subscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(disposingMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => System.Diagnostics.Trace.TraceInformation(disposedMessage)));

				System.Diagnostics.Trace.TraceInformation(subscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, TraceSource trace)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				trace.TraceInformation(Rxx.Properties.Text.DefaultSubscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => trace.TraceInformation(Rxx.Properties.Text.DefaultDisposingSubscriptionMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => trace.TraceInformation(Rxx.Properties.Text.DefaultDisposedSubscriptionMessage)));

				trace.TraceInformation(Rxx.Properties.Text.DefaultSubscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, TraceSource trace, string name)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(name != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			return source.TraceSubscriptions(
				trace,
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.SubscribingFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.SubscribedFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.DisposingSubscriptionFormat, name),
				string.Format(CultureInfo.CurrentCulture, Rxx.Properties.Text.DisposedSubscriptionFormat, name));
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, TraceSource trace, string subscribingMessage, string subscribedMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(subscribingMessage != null);
			Contract.Requires(subscribedMessage != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				trace.TraceInformation(subscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => trace.TraceInformation(Rxx.Properties.Text.DefaultDisposingSubscriptionMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => trace.TraceInformation(Rxx.Properties.Text.DefaultDisposedSubscriptionMessage)));

				trace.TraceInformation(subscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Subscription is returned to observer.")]
		public static IObservable<T> TraceSubscriptions<T>(this IObservable<T> source, TraceSource trace, string subscribingMessage, string subscribedMessage, string disposingMessage, string disposedMessage)
		{
			Contract.Requires(source != null);
			Contract.Requires(trace != null);
			Contract.Requires(subscribingMessage != null);
			Contract.Requires(subscribedMessage != null);
			Contract.Requires(disposingMessage != null);
			Contract.Requires(disposedMessage != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(observer =>
			{
				trace.TraceInformation(subscribingMessage);

				var subscription = new CompositeDisposable(
					Disposable.Create(() => trace.TraceInformation(disposingMessage)),
					source.Subscribe(observer),
					Disposable.Create(() => trace.TraceInformation(disposedMessage)));

				trace.TraceInformation(subscribedMessage);

				return subscription;
			});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
