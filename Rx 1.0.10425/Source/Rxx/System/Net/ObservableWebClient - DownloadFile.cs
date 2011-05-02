using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		/// <summary>
		/// Downloads the specified resource as a file.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <param name="fileName">The file to create or overwrite with the resource.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
		public static IObservable<string> DownloadFile(
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

		/// <summary>
		/// Downloads the specified resource as a file and includes a channel for progress notifications.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <param name="fileName">The file to create or overwrite with the resource.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the result of the 
		/// download in the right channel and replays the result to observers.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a file.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <param name="fileName">The file to create or overwrite with the resource.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a file and includes a channel for progress notifications.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <param name="fileName">The file to create or overwrite with the resource.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the result of the 
		/// download in the right channel and replays the result to observers.</returns>
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
