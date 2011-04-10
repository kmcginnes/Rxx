using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<byte[]> UploadDataObservable(
			Uri address,
			string method,
			byte[] data)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadDataObservable(client, address, method, data));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadDataWithProgress(
			Uri address,
			string method,
			byte[] data)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadDataWithProgress(client, address, method, data));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<byte[]> UploadDataObservable(
			this WebClient client,
			Uri address,
			string method,
			byte[] data)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<UploadDataCompletedEventHandler, UploadDataCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadDataCompleted += handler,
				handler => client.UploadDataCompleted -= handler,
				token => client.UploadDataAsync(address, method, data, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadDataWithProgress(
			this WebClient client,
			Uri address,
			string method,
			byte[] data)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			return Observable2.FromEventBasedAsyncPattern<UploadDataCompletedEventHandler, UploadDataCompletedEventArgs, UploadProgressChangedEventHandler, UploadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadDataCompleted += handler,
				handler => client.UploadDataCompleted -= handler,
				handler => handler.Invoke,
				handler => client.UploadProgressChanged += handler,
				handler => client.UploadProgressChanged -= handler,
				token => client.UploadDataAsync(address, method, data, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => right.EventArgs.Result);
		}
	}
}
