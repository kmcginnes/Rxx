using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		public static IObservable<byte[]> UploadValuesObservable(
			Uri address,
			string method,
			NameValueCollection values)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(values != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadValuesObservable(client, address, method, values));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadValuesWithProgress(
			Uri address,
			string method,
			NameValueCollection values)
		{
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(values != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadValuesWithProgress(client, address, method, values));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		public static IObservable<byte[]> UploadValuesObservable(
			this WebClient client,
			Uri address,
			string method,
			NameValueCollection values)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(values != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable2.FromEventBasedAsyncPattern<UploadValuesCompletedEventHandler, UploadValuesCompletedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadValuesCompleted += handler,
				handler => client.UploadValuesCompleted -= handler,
				token => client.UploadValuesAsync(address, method, values, token),
				client.CancelAsync)
				.Select(e => e.EventArgs.Result);

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadValuesWithProgress(
			this WebClient client,
			Uri address,
			string method,
			NameValueCollection values)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
			Contract.Requires(method != null);
			Contract.Requires(values != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			return Observable2.FromEventBasedAsyncPattern<UploadValuesCompletedEventHandler, UploadValuesCompletedEventArgs, UploadProgressChangedEventHandler, UploadProgressChangedEventArgs>(
				handler => handler.Invoke,
				handler => client.UploadValuesCompleted += handler,
				handler => client.UploadValuesCompleted -= handler,
				handler => handler.Invoke,
				handler => client.UploadProgressChanged += handler,
				handler => client.UploadProgressChanged -= handler,
				token => client.UploadValuesAsync(address, method, values, token),
				client.CancelAsync)
				.Select(
					left => left.EventArgs,
					right => right.EventArgs.Result);
		}
	}
}
