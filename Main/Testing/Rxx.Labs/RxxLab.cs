using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using DaveSexton.Labs;

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
		#endregion
	}
}
