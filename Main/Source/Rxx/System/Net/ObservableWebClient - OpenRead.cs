using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<Stream> OpenReadObservable(
			Uri address)
		{
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<Stream>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => OpenReadObservable(client, address));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, Stream> OpenReadWithProgress(
			Uri address)
		{
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, Stream>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => OpenReadWithProgress(client, address));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<Stream> OpenReadObservable(
			this WebClient client,
			Uri address)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<Stream>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<OpenReadCompletedEventHandler, OpenReadCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.OpenReadCompleted += handler,
				handler => client.OpenReadCompleted -= handler,
				token => client.OpenReadAsync(address, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, Stream> OpenReadWithProgress(
			this WebClient client,
			Uri address)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, Stream>>() != null);

			return Observable2.FromEventBasedAsyncPattern<OpenReadCompletedEventHandler, OpenReadCompletedEventArgs, DownloadProgressChangedEventHandler, DownloadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.OpenReadCompleted += handler,
				handler => client.OpenReadCompleted -= handler,
				handler => handler.Invoke,
				handler => client.DownloadProgressChanged += handler,
				handler => client.DownloadProgressChanged -= handler,
				token => client.OpenReadAsync(address, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => right.EventArgs.Result);
		}
	}
}
