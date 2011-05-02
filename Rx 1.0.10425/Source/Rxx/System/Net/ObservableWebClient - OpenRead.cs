using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		/// <summary>
		/// Downloads the specified resource as a <see cref="Stream"/>.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable containing the readable stream that reads data from the resource.</returns>
		public static IObservable<Stream> OpenRead(
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Stream"/> and includes a channel for progress notifications.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel and 
		/// contains the readable stream that reads data from the resource in the right channel.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Stream"/>.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable containing the readable stream that reads data from the resource.</returns>
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

		/// <summary>
		/// Downloads the specified resource as a <see cref="Stream"/> and includes a channel for progress notifications.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel and 
		/// contains the readable stream that reads data from the resource in the right channel.</returns>
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
