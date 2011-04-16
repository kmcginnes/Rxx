using System.Diagnostics.Contracts;
using System.Disposables;
using System.Linq;

namespace System.Net
{
	public static class ObservableHttpListener
	{
		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(endPoint, null);
		}

		public static IObservable<HttpListenerContext> Start(IPEndPoint endPoint, int maxConcurrent)
		{
			Contract.Requires(endPoint != null);
			Contract.Requires(endPoint.Address != null);
			Contract.Requires(endPoint.Port >= IPEndPoint.MinPort && endPoint.Port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(endPoint, null, maxConcurrent);
		}

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

		public static IObservable<HttpListenerContext> Start(string domain)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, 80, null);
		}

		public static IObservable<HttpListenerContext> Start(string domain, int port)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, port, null);
		}

		public static IObservable<HttpListenerContext> Start(string domain, int port, int maxConcurrent)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, port, null, maxConcurrent);
		}

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

		public static IObservable<HttpListenerContext> StartObservable(this HttpListener listener)
		{
			Contract.Requires(listener != null);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			Contract.Assume(Observable2.DefaultMaxConcurrent > 0);

			return StartObservable(listener, Observable2.DefaultMaxConcurrent);
		}

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
