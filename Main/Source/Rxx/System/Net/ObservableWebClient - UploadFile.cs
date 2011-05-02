using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Net
{
	public static partial class ObservableWebClient
	{
		/// <summary>
		/// Uploads a file to the specified resource.
		/// </summary>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send data to the resource.  If <see langword="null"/>, the default is POST for HTTP and STOR for FTP.</param>
		/// <param name="fileName">The file to upload to the resource.</param>
		/// <returns>An observable that caches the response from the server and replays it to observers.</returns>
		public static IObservable<byte[]> UploadFile(
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IObservable<byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadFileObservable(client, address, method, fileName));

			Contract.Assume(observable != null);

			return observable;
		}

		/// <summary>
		/// Uploads a file to the specified resource and includes a channel for progress notifications.
		/// </summary>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send data to the resource.  If <see langword="null"/>, the default is POST for HTTP and STOR for FTP.</param>
		/// <param name="fileName">The file to upload to the resource.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the response from the 
		/// server in the right channel and replays the response to observers.</returns>
		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadFileWithProgress(
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(address != null);
			Contract.Requires(fileName != null);
			Contract.Ensures(Contract.Result<IPairedObservable<UploadProgressChangedEventArgs, byte[]>>() != null);

			var observable = Observable.Using(
				() => new WebClient(),
				client => UploadFileWithProgress(client, address, method, fileName));

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		/// <summary>
		/// Uploads a file to the specified resource.
		/// </summary>
		/// <param name="client">The object that uploads to the resource.</param>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send data to the resource.  If <see langword="null"/>, the default is POST for HTTP and STOR for FTP.</param>
		/// <param name="fileName">The file to upload to the resource.</param>
		/// <returns>An observable that caches the response from the server and replays it to observers.</returns>
		public static IObservable<byte[]> UploadFileObservable(
			this WebClient client,
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
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

		/// <summary>
		/// Uploads a file to the specified resource and includes a channel for progress notifications.
		/// </summary>
		/// <param name="client">The object that uploads to the resource.</param>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send data to the resource.  If <see langword="null"/>, the default is POST for HTTP and STOR for FTP.</param>
		/// <param name="fileName">The file to upload to the resource.</param>
		/// <returns>A paired observable that pushes progress notifications through the left channel, caches the response from the 
		/// server in the right channel and replays the response to observers.</returns>
		public static IPairedObservable<UploadProgressChangedEventArgs, byte[]> UploadFileWithProgress(
			this WebClient client,
			Uri address,
			string method,
			string fileName)
		{
			Contract.Requires(client != null);
			Contract.Requires(address != null);
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
