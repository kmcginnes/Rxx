using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<string> UploadString(
			Uri address,
			string method,
			string data)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadStringObservable(client, address, method, data));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<string> UploadStringObservable(
			this WebClient client,
			Uri address,
			string method,
			string data)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Ensures(Contract.Result<IObservable<string>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<UploadStringCompletedEventHandler, UploadStringCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadStringCompleted += handler,
				handler => client.UploadStringCompleted -= handler,
				token => client.UploadStringAsync(address, method, data, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
