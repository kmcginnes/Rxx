using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<byte[]> DownloadData(
			Uri address)
		{
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => DownloadDataObservable(client, address));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, byte[]> DownloadDataWithProgress(
			Uri address)
		{
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => DownloadDataWithProgress(client, address));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<byte[]> DownloadDataObservable(
			this WebClient client,
			Uri address)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<DownloadDataCompletedEventHandler, DownloadDataCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.DownloadDataCompleted += handler,
				handler => client.DownloadDataCompleted -= handler,
				token => client.DownloadDataAsync(address, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, byte[]> DownloadDataWithProgress(
			this WebClient client,
			Uri address)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, byte[]>>() != null);

			return Observable2.FromEventBasedAsyncPattern<DownloadDataCompletedEventHandler, DownloadDataCompletedEventArgs, DownloadProgressChangedEventHandler, DownloadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.DownloadDataCompleted += handler,
				handler => client.DownloadDataCompleted -= handler,
				handler => handler.Invoke,
				handler => client.DownloadProgressChanged += handler,
				handler => client.DownloadProgressChanged -= handler,
				token => client.DownloadDataAsync(address, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => right.EventArgs.Result);
		}
	}
}
