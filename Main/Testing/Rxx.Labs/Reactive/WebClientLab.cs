using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Reactive WebClient")]
	[Description("Examples of making web requests using ObservableWebClient and ObservableHttpListener.")]
	public sealed class WebClientLab : RxxLab
	{
		private void DownloadHtmlExperiment()
		{
			Uri address = UserInputUrl(Text.PromptFormat, Instructions.EnterAUrl);

			TraceLine(Instructions.PressAnyKeyToCancel);

			using (var client = new WebClient())
			{
				var htmlTagsWithIds = client
					.DownloadStringObservable(address)
					.SelectMany(response =>
						Regex.Matches(
							response,
							@"\< (?<Tag> \w+? ) \s [^\>]*? id= (?<Q> [""'] ) (?<ID> .+? ) \k<Q> .*? \>",
								RegexOptions.IgnoreCase
							| RegexOptions.ExplicitCapture
							| RegexOptions.IgnorePatternWhitespace
							| RegexOptions.Singleline)
						.Cast<Match>())
					.Select(match => new
					{
						Tag = match.Groups["Tag"].Value,
						Id = match.Groups["ID"].Value
					});

				using (htmlTagsWithIds.Subscribe(ConsoleOutput))
				{
					Console.ReadKey();
				}
			}
		}

		private void DownloadProgressExperiment()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			var address = new IPEndPoint(IPAddress.Loopback, 15005);

			using (ObservableHttpListener.Start(address).Subscribe(
				context =>
				{
					using (var response = context.Response)
					{
						var encoding = Encoding.UTF8;

						response.StatusCode = (int) HttpStatusCode.OK;
						response.ContentType = "text/plain";
						response.ContentEncoding = encoding;

						string message = UserInput(Text.PromptFormat, Instructions.EnterTheResponse);

						const int bytesPerBatch = 100 * 1000;
						const int batchCount = 5;

						int repeatCount = bytesPerBatch / message.Length;

						response.ContentLength64 = batchCount * repeatCount * message.Length;

						var stream = response.OutputStream;

						for (int i = 0; i < batchCount; i++)
						{
							var oneHundredKB = new string(message.Repeat(repeatCount).ToArray());
							var bytes = encoding.GetBytes(oneHundredKB);

							stream.Write(bytes, 0, bytes.Length);
							stream.Flush();

							System.Threading.Thread.Sleep(TimeSpan.FromSeconds(.5));
						}
					}
				}))
			{
				TraceLine();
				TraceLine(Text.Subscribing);
				TraceLine();

				var url = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + address.ToString());

				using (ObservableWebClient.DownloadDataWithProgress(url).Subscribe(
					ConsoleOutputOnNext<DownloadProgressChangedEventArgs>(
						"Progress",
						progress => string.Format(
							System.Globalization.CultureInfo.CurrentCulture,
							"{0,5:P0} - {1} of {2} byte(s)",
							progress.ProgressPercentage / 100f,
							progress.BytesReceived,
							progress.TotalBytesToReceive)),
					ConsoleOutputOnNext<byte[]>("Result", bytes => bytes.Length + " byte(s)"),
					ConsoleOutputOnError(),
					ConsoleOutputOnCompleted()))
				{
					Console.ReadKey();
				}
			}
		}

		protected override void Main()
		{
			DownloadHtmlExperiment();

			TraceLine();

			DownloadProgressExperiment();
		}
	}
}