using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(watch != null);
		}

		public void StartTimer()
		{
			watch.Restart();
		}
		#endregion

		#region IObserver<T> Members
		public void OnNext(T value)
		{
			hasValue = true;

			if (showValues)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;

				if (showTimeOnNext)
				{
					Console.WriteLine(Text.OnNextTimeFormat, watch.Elapsed, value);
				}
				else
				{
					Console.WriteLine(value);
				}

				Console.ResetColor();
			}
		}

		public void OnError(Exception error)
		{
			watch.Stop();

			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine(Text.OnErrorTimeFormat, watch.Elapsed);
			Console.WriteLine(error.Message);

			Console.ResetColor();
		}

		public void OnCompleted()
		{
			watch.Stop();

			if (!hasValue)
			{
				Console.ForegroundColor = ConsoleColor.Magenta;

				Console.WriteLine(Text.OnCompletedEmpty);
				Console.WriteLine();
			}

			Console.ForegroundColor = ConsoleColor.Green;

			Console.WriteLine(Text.OnCompletedTimeFormat, watch.Elapsed);

			Console.ResetColor();
		}
		#endregion
	}
}
