using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<string> DownloadFileObservable(
			Uri address,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => DownloadFileObservable(client, address, fileName));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, string> DownloadFileWithProgress(
			Uri address,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, string>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => DownloadFileWithProgress(client, address, fileName));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<string> DownloadFileObservable(
			this WebClient client,
			Uri address,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<AsyncCompletedEventHandler, AsyncCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.DownloadFileCompleted += handler,
				handler => client.DownloadFileCompleted -= handler,
				token => client.DownloadFileAsync(address, fileName, token),
				client.CancelAsync)
				.Select(e => fileName);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<DownloadProgressChangedEventArgs, string> DownloadFileWithProgress(
			this WebClient client,
			Uri address,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IPairedObservable<DownloadProgressChangedEventArgs, string>>() != null);

			return Observable2.FromEventBasedAsyncPattern<AsyncCompletedEventHandler, AsyncCompletedEventArgs, DownloadProgressChangedEventHandler, DownloadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.DownloadFileCompleted += handler,
				handler => client.DownloadFileCompleted -= handler,
				handler => handler.Invoke,
				handler => client.DownloadProgressChanged += handler,
				handler => client.DownloadProgressChanged -= handler,
				token => client.DownloadFileAsync(address, fileName, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => fileName);
		}
	}
}
