using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using Rxx.Labs.Properties;

namespace Rxx.Labs
{
	internal sealed class ConsoleObserver<T> : IObserver<T>
	{
		#region Public Properties
		public TimeSpan Elapsed
		{
			get
			{
				return watch.Elapsed;
			}
		}
		#endregion

		#region Private / Protected
		private readonly Stopwatch watch = new Stopwatch();
		private readonly bool showTimeOnNext, showValues;
		private readonly string name, valueFormat;
		private bool hasValue;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the <see cref="ConsoleObserver" /> class.
		/// </summary>
		public ConsoleObserver()
			: this(showTimeOnNext: true)
		{
		}

		/// <summary>
		/// Constructs a new instance of the <see cref="ConsoleObserver" /> class.
		/// </summary>
		public ConsoleObserver(bool showTimeOnNext)
		{
			this.showTimeOnNext = showTimeOnNext;
			this.showValues = !(typeof(T) == typeof(Unit));
		}

		/// <summary>
		/// Constructs a new instance of the <see cref="ConsoleObserver" /> class.
		/// </summary>
		public ConsoleObserver(string name)
			: this(name, showTimeOnNext: true)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));
		}

		/// <summary>
		/// Constructs a new instance of the <see cref="ConsoleObserver" /> class.
		/// </summary>
		public ConsoleObserver(string name, bool showTimeOnNext)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));

			this.name = name;
			this.showTimeOnNext = showTimeOnNext;
			this.showValues = !(typeof(T) == typeof(Unit));
		}

		/// <summary>
		/// Constructs a new instance of the <see cref="ConsoleObserver" /> class.
		/// </summary>
		internal ConsoleObserver(string name, string valueFormat, bool showTimeOnNext)
		{
			Contract.Requires(!string.IsNullOrEmpty(valueFormat));

			this.name = name;
			this.valueFormat = valueFormat;
			this.showTimeOnNext = showTimeOnNext;
			this.showValues = !(typeof(T) == typeof(Unit));
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(watch != null);
		}

		public static ConsoleObserver<T> Error()
		{
			Contract.Ensures(Contract.Result<ConsoleObserver<T>>() != null);

			return new ConsoleObserver<T>()
			{
				hasValue = true		// assumption
			};
		}

		public static ConsoleObserver<T> Error(string name)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Ensures(Contract.Result<ConsoleObserver<T>>() != null);

			return new ConsoleObserver<T>(name)
			{
				hasValue = true		// assumption
			};
		}

		public static ConsoleObserver<T> Completed()
		{
			Contract.Ensures(Contract.Result<ConsoleObserver<T>>() != null);

			return new ConsoleObserver<T>()
			{
				hasValue = true		// assumption
			};
		}

		public static ConsoleObserver<T> Completed(string name)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Ensures(Contract.Result<ConsoleObserver<T>>() != null);

			return new ConsoleObserver<T>(name)
			{
				hasValue = true		// assumption
			};
		}

		public void StartTimer()
		{
			watch.Restart();
		}

		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)",
			Justification = "Single whitespace.")]
		private void WriteName()
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				Console.Write(name);
				Console.Write(" ");
			}
		}
		#endregion

		#region IObserver<T> Members
		public void OnNext(T value)
		{
			hasValue = true;

			if (showValues)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;

				WriteName();

				if (showTimeOnNext)
				{
					if (valueFormat != null)
						Console.WriteLine(Text.OnNextTimeFormat, watch.Elapsed, string.Format(CultureInfo.CurrentCulture, valueFormat, value));
					else
						Console.WriteLine(Text.OnNextTimeFormat, watch.Elapsed, value);
				}
				else
				{
					if (valueFormat != null)
						Console.WriteLine(string.Format(CultureInfo.CurrentCulture, valueFormat, value));
					else
						Console.WriteLine(value);
				}

				Console.ResetColor();
			}
		}

		public void OnError(Exception error)
		{
			watch.Stop();

			Console.ForegroundColor = ConsoleColor.Red;

			WriteName();

			Console.WriteLine(Text.OnErrorTimeFormat, watch.Elapsed, error.Message);

			Console.ResetColor();
		}

		public void OnCompleted()
		{
			watch.Stop();

			if (!hasValue)
			{
				Console.ForegroundColor = ConsoleColor.Magenta;

				WriteName();

				Console.WriteLine(Text.OnCompletedEmpty);
				Console.WriteLine();
			}

			Console.ForegroundColor = ConsoleColor.Green;

			WriteName();

			Console.WriteLine(Text.OnCompletedTimeFormat, watch.Elapsed);

			Console.ResetColor();
		}
		#endregion
	}
}
