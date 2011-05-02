using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		/// <summary>
		/// Downloads the specified resource as a <see cref="string"/>.
		/// </summary>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
		public static IObservable<string> DownloadString(
			Uri address)
		{
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => DownloadStringObservable(client, address));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Downloads the specified resource as a <see cref="string"/>.
		/// </summary>
		/// <param name="client">The object that downloads the resource.</param>
		/// <param name="address">A <see cref="Uri"/> containing the URI to download.</param>
		/// <returns>An observable that caches the result of the download and replays it to observers.</returns>
		public static IObservable<string> DownloadStringObservable(
			this WebClient client,
			Uri address)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<DownloadStringCompletedEventHandler, DownloadStringCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.DownloadStringCompleted += handler,
				handler => client.DownloadStringCompleted -= handler,
				token => client.DownloadStringAsync(address, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
