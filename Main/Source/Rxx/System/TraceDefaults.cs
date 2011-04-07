using System.Diagnostics.Contracts;
using System.Globalization;

namespace System
{
	public static class TraceDefaults
	{
		public static string DefaultOnNext<T>(T value)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return value == null ? string.Empty : value.ToString();
		}

		public static string DefaultOnError(Exception exception)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return exception == null ? string.Empty : exception.ToString();
		}

		public static string DefaultOnCompleted()
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return Rxx.Properties.Text.DefaultOnCompletedMessage;
		}

		public static string DefaultOnNext<T>(int observerId, T value)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return FormatMessage(observerId, DefaultOnNext(value));
		}

		public static string DefaultOnError(int observerId, Exception exception)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return FormatMessage(observerId, DefaultOnError(exception));
		}

		public static string DefaultOnCompleted(int observerId)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return FormatMessage(observerId, DefaultOnCompleted());
		}

		public static string FormatMessage(int observerId, string message)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return observerId + ":" + message;
		}

		internal static Func<T, string> GetFormatOnNext<T>(string nextFormat)
		{
			Contract.Requires(nextFormat != null);
			Contract.Ensures(Contract.Result<Func<T, string>>() != null);

			return value => string.Format(CultureInfo.CurrentCulture, nextFormat, value);
		}

		internal static Func<Exception, string> GetFormatOnError(string errorFormat)
		{
			Contract.Requires(errorFormat != null);
			Contract.Ensures(Contract.Result<Func<Exception, string>>() != null);

			return error => string.Format(CultureInfo.CurrentCulture, errorFormat, error);
		}

		internal static Func<string> GetMessageOnCompleted(string completedMessage)
		{
			Contract.Requires(completedMessage != null);
			Contract.Ensures(Contract.Result<Func<string>>() != null);

			return () => completedMessage;
		}

		internal static Func<int, T, string> GetIdentityFormatOnNext<T>(string nextFormat)
		{
			Contract.Requires(nextFormat != null);
			Contract.Ensures(Contract.Result<Func<int, T, string>>() != null);

			return (id, value) => string.Format(CultureInfo.CurrentCulture, nextFormat, id, value);
		}

		internal static Func<int, Exception, string> GetIdentityFormatOnError(string errorFormat)
		{
			Contract.Requires(errorFormat != null);
			Contract.Ensures(Contract.Result<Func<int, Exception, string>>() != null);

			return (id, error) => string.Format(CultureInfo.CurrentCulture, errorFormat, id, error);
		}

		internal static Func<int, string> GetIdentityMessageOnCompleted(string completedMessage)
		{
			Contract.Requires(completedMessage != null);
			Contract.Ensures(Contract.Result<Func<int, string>>() != null);

			return id => string.Format(CultureInfo.CurrentCulture, completedMessage, id);
		}
	}
}
