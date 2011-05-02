using System.Diagnostics.Contracts;
using System.Disposables;
using System.Linq;

namespace System.Net
{
	/// <summary>
	/// Provides methods for creating HTTP request observables.
	/// </summary>
	public static class ObservableHttpListener
	{
		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(IPEndPoint)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="endPoint">The local end point on which to listen for requests.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(endPoint, null);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(IPEndPoint,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="endPoint">The local end point on which to listen for requests.</param>
		/// <param name="maxConcurrent">The maximum number of requests that can be pushed through the observable simultaneously.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint, int maxConcurrent)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(endPoint, null, maxConcurrent);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(IPEndPoint,string)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="endPoint">The local end point on which to listen for requests.</param>
		/// <param name="path">The path at which requests will be served.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint, string path)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			var address = endPoint.Address.ToString();

			Contract.Assume(!string.IsNullOrEmpty(address));

			return Start(address, endPoint.Port, path);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(IPEndPoint,string,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="endPoint">The local end point on which to listen for requests.</param>
		/// <param name="path">The path at which requests will be served.</param>
		/// <param name="maxConcurrent">The maximum number of requests that can be pushed through the observable simultaneously.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint, string path, int maxConcurrent)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			var address = endPoint.Address.ToString();

			Contract.Assume(!string.IsNullOrEmpty(address));

			return Start(address, endPoint.Port, path, maxConcurrent);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(string)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="domain">The domain name on which to listen for requests.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(string domain)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, 80, null);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(string,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="domain">The domain name on which to listen for requests.</param>
		/// <param name="port">The port number on which to listen for requests.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(string domain, int port)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, port, null);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(string,int,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="domain">The domain name on which to listen for requests.</param>
		/// <param name="port">The port number on which to listen for requests.</param>
		/// <param name="maxConcurrent">The maximum number of requests that can be pushed through the observable simultaneously.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> Start(string domain, int port, int maxConcurrent)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, port, null, maxConcurrent);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(string,int,string)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="domain">The domain name on which to listen for requests.</param>
		/// <param name="port">The port number on which to listen for requests.</param>
		/// <param name="path">The path at which requests will be served.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "The listener variable is disposed by Observable.Using.")]
		public static IObservable<HttpListenerContext> Start(string domain, int port, string path)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			Contract.Assume(Observable2.DefaultMaxConcurrent > 0);

			return Start(domain, port, path, Observable2.DefaultMaxConcurrent);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="Start(string,int,string,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="domain">The domain name on which to listen for requests.</param>
		/// <param name="port">The port number on which to listen for requests.</param>
		/// <param name="path">The path at which requests will be served.</param>
		/// <param name="maxConcurrent">The maximum number of requests that can be pushed through the observable simultaneously.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
					Justification = "The listener variable is disposed by Observable.Using.")]
		public static IObservable<HttpListenerContext> Start(string domain, int port, string path, int maxConcurrent)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			var url = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + domain + ":" + port + "/" + path);

			var observable = Observable.Using(
				() =>
				{
					var listener = new HttpListener();

					listener.Prefixes.Add(url.ToString());

					return listener;
				},
				listener => StartObservable(listener, maxConcurrent));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="StartObservable(HttpListener)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="listener">The object that listens for requests.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		public static IObservable<HttpListenerContext> StartObservable(this HttpListener listener)
		{
			Contract.Requires(listener != null);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			Contract.Assume(Observable2.DefaultMaxConcurrent > 0);

			return StartObservable(listener, Observable2.DefaultMaxConcurrent);
		}

		/// <summary>
		/// Returns an observable of concurrent HTTP requests.
		/// </summary>
		/// <remarks>
		/// <alert type="warn">
		/// <see cref="StartObservable(HttpListener,int)"/> does not guarantee the serializability of notifications that is recommended in the 
		/// Rx Design Guidelines.  This allows observers to receive multiple requests concurrently, as is common in 
		/// hosting scenarios.
		/// </alert>
		/// </remarks>
		/// <param name="listener">The object that listens for requests.</param>
		/// <param name="maxConcurrent">The maximum number of requests that can be pushed through the observable simultaneously.</param>
		/// <returns>An observable of concurrent HTTP requests.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Exception is passed to observers.")]
		public static IObservable<HttpListenerContext> StartObservable(this HttpListener listener, int maxConcurrent)
		{
			Contract.Requires(listener != null);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			var getRequest = Observable.FromAsyncPattern<HttpListenerContext>(listener.BeginGetContext, listener.EndGetContext);

			Contract.Assume(getRequest != null);

			var observable = Observable.CreateWithDisposable<HttpListenerContext>(
				observer =>
				{
					try
					{
						if (!listener.IsListening)
						{
							listener.Start();
						}
					}
					catch (Exception ex)
					{
						observer.OnError(ex);

						return Disposable.Empty;
					}

					return getRequest
						.Serve(maxConcurrent)
						.Finally(() =>
						{
							try
							{
								listener.Stop();
							}
							catch (ObjectDisposedException)
							{
							}
						})
						.Subscribe(observer);
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
