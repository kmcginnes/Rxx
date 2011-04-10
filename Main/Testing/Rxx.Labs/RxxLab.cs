using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using DaveSexton.Labs;
using Rxx.Labs.Properties;

namespace Rxx.Labs
{
	public abstract class RxxLab : Lab
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		protected bool ShowTimeOnNext
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="RxxLab" /> class.
		/// </summary>
		protected RxxLab()
		{
			ShowTimeOnNext = true;
		}
		#endregion

		#region Methods
		protected static void TraceDescription(string description)
		{
			TraceLine(ConsoleFormat.Wrap("  ", description));
			TraceLine();
		}

		protected virtual string UserInput(string format, params object[] args)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(format));
			Contract.Requires(args != null);
			Contract.Ensures(Contract.Result<string>() != null);

			Console.Write(format, args);

			return Console.ReadLine();
		}

		protected Uri UserInputUrl(string format, params object[] args)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(format));
			Contract.Requires(args != null);
			Contract.Ensures(Contract.Result<Uri>() != null);
			Contract.Ensures(Contract.Result<Uri>().IsAbsoluteUri);

			Uri url;
			while (!Uri.TryCreate(UserInput(format, args), UriKind.Absolute, out url))
			{
				Console.WriteLine();
				TraceError(Text.InvalidUrl);
				Console.WriteLine();
			}

			return url;
		}

		protected virtual IObserver<object> ConsoleOutput()
		{
			Contract.Ensures(Contract.Result<IObserver<object>>() != null);

			var observer = new ConsoleObserver<object>(ShowTimeOnNext);

			observer.StartTimer();

			return observer;
		}

		protected virtual Func<IObserver<object>> ConsoleOutput(string name)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			Contract.Ensures(Contract.Result<Func<IObserver<object>>>() != null);

			return () =>
			{
				Contract.Ensures(Contract.Result<IObserver<object>>() != null);

				var observer = new ConsoleObserver<object>(name, ShowTimeOnNext);

				observer.StartTimer();

				return observer;
			};
		}

		protected virtual Func<IObserver<object>> ConsoleOutput(string nameFormat, params object[] args)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(nameFormat));
			Contract.Requires(args != null);
			Contract.Ensures(Contract.Result<Func<IObserver<object>>>() != null);

			string name = string.Format(CultureInfo.CurrentCulture, nameFormat, args);

			Contract.Assume(!string.IsNullOrWhiteSpace(name));

			return ConsoleOutput(name);
		}

		protected virtual Func<IObserver<object>> ConsoleOutputFormat(string valueFormat)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(valueFormat));
			Contract.Ensures(Contract.Result<Func<IObserver<object>>>() != null);

			return () =>
			{
				Contract.Ensures(Contract.Result<IObserver<object>>() != null);

				var observer = new ConsoleObserver<object>(null, valueFormat, ShowTimeOnNext);

				observer.StartTimer();

				return observer;
			};
		}

		protected virtual Func<IObserver<object>> ConsoleOutputFormat(string name, string valueFormat)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			Contract.Requires(!string.IsNullOrWhiteSpace(valueFormat));
			Contract.Ensures(Contract.Result<Func<IObserver<object>>>() != null);

			return () =>
			{
				Contract.Ensures(Contract.Result<IObserver<object>>() != null);

				var observer = new ConsoleObserver<object>(name, valueFormat, ShowTimeOnNext);

				observer.StartTimer();

				return observer;
			};
		}

		protected virtual Action<T> ConsoleOutputOnNext<T>(Func<T, string> format)
		{
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<Action<T>>() != null);

			var observer = new ConsoleObserver<object>(ShowTimeOnNext);

			observer.StartTimer();

			return value => observer.OnNext(format(value));
		}

		protected virtual Action<Exception> ConsoleOutputOnError()
		{
			Contract.Ensures(Contract.Result<Action<Exception>>() != null);

			var observer = ConsoleObserver<object>.Error();

			observer.StartTimer();

			return ex => observer.OnError(ex);
		}

		protected virtual Action ConsoleOutputOnCompleted()
		{
			Contract.Ensures(Contract.Result<Action>() != null);

			var observer = ConsoleObserver<object>.Completed();

			observer.StartTimer();

			return observer.OnCompleted;
		}

		protected virtual Action<T> ConsoleOutputOnNext<T>(string name, Func<T, string> format)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			Contract.Requires(format != null);
			Contract.Ensures(Contract.Result<Action<T>>() != null);

			var observer = new ConsoleObserver<object>(name, ShowTimeOnNext);

			observer.StartTimer();

			return value => observer.OnNext(format(value));
		}

		protected virtual Action<Exception> ConsoleOutputOnError(string name)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			Contract.Ensures(Contract.Result<Action<Exception>>() != null);

			var observer = ConsoleObserver<object>.Error(name);

			observer.StartTimer();

			return ex => observer.OnError(ex);
		}

		protected virtual Action ConsoleOutputOnCompleted(string name)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(name));
			Contract.Ensures(Contract.Result<Action>() != null);

			var observer = ConsoleObserver<object>.Completed(name);

			observer.StartTimer();

			return observer.OnCompleted;
		}
		#endregion
	}
}
