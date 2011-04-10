using System.Diagnostics.Contracts;
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

		public static IObservable<HttpListenerContext> Start(string domain, int port)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			return Start(domain, port, null);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "The listener variable is disposed by Observable.Using.")]
		public static IObservable<HttpListenerContext> Start(string domain, int port, string path)
		{
			Contract.Requires(!string.IsNullOrEmpty(domain));
			Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			var url = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + domain + ":" + port + "/" + path);

			var observable = Observable.Using(
				() =>
				{
					var listener = new HttpListener();

					listener.Prefixes.Add(url.ToString());

					return listener;
				},
				StartObservable);

			Contract.Assume(observable != null);

			return observable;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Exception is passed to observers.")]
		public static IObservable<HttpListenerContext> StartObservable(this HttpListener listener)
		{
			Contract.Requires(listener != null);
			Contract.Ensures(Contract.Result<IObservable<HttpListenerContext>>() != null);

			IObservable<HttpListenerContext> observable;

			try
			{
				var getRequest = Observable.FromAsyncPattern<HttpListenerContext>(listener.BeginGetContext, listener.EndGetContext);

				Contract.Assume(getRequest != null);

				listener.Start();

				observable = getRequest().Concat(Observable.Defer(getRequest).Repeat());
			}
			catch (Exception ex)
			{
				observable = Observable.Throw<HttpListenerContext>(ex);
			}

			observable = observable.Finally(() =>
				{
					try
					{
						listener.Stop();
					}
					catch (ObjectDisposedException)
					{
					}
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
