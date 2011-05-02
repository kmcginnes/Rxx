using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	/// <summary>
	/// Provides common methods for asynchronously sending data to and observing data from a resource identified by a URI.
	/// </summary>
	public static partial class ObservableWebClient
	{
		/// <summary>
		/// Downloads the specified resource as a <see cref="Byte"/> array.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Byte"/> array and includes a channel for progress notifications.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the result of the 
		/// download in the right channel and replays the result to observers.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Byte"/> array.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Byte"/> array and includes a channel for progress notifications.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the result of the 
		/// download in the right channel and replays the result to observers.</returns>
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
