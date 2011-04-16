using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
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
