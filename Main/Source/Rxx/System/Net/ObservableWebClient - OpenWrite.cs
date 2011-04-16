using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<Stream> OpenWrite(
			Uri address,
			string method)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Ensures(Contract.Result<IObservable<Stream>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => OpenWriteObservable(client, address, method));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<Stream> OpenWriteObservable(
			this WebClient client,
			Uri address,
			string method)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Ensures(Contract.Result<IObservable<Stream>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<OpenWriteCompletedEventHandler, OpenWriteCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.OpenWriteCompleted += handler,
				handler => client.OpenWriteCompleted -= handler,
				token => client.OpenWriteAsync(address, method, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
