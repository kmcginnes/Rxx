using System;
using System.Diagnostics.Contracts;
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
		protected virtual IObserver<object> ConsoleOutput()
		{
			Contract.Ensures(Contract.Result<IObserver<object>>() != null);

			var observer = new ConsoleObserver<object>(ShowTimeOnNext);

			observer.StartTimer();

			return observer;
		}
		#endregion
	}
}
