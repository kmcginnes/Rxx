using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<byte[]> UploadFile(
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadFileObservable(client, address, method, fileName));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadFileWithProgress(
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadFileWithProgress(client, address, method, fileName));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<byte[]> UploadFileObservable(
			this WebClient client,
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<UploadFileCompletedEventHandler, UploadFileCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadFileCompleted += handler,
				handler => client.UploadFileCompleted -= handler,
				token => client.UploadFileAsync(address, method, fileName, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadFileWithProgress(
			this WebClient client,
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			return Observable2.FromEventBasedAsyncPattern<UploadFileCompletedEventHandler, UploadFileCompletedEventArgs, UploadProgressChangedEventHandler, UploadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadFileCompleted += handler,
				handler => client.UploadFileCompleted -= handler,
				handler => handler.Invoke,
				handler => client.UploadProgressChanged += handler,
				handler => client.UploadProgressChanged -= handler,
				token => client.UploadFileAsync(address, method, fileName, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => right.EventArgs.Result);
		}
	}
}
