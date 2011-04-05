using System;
using System.ComponentModel;
using System.Linq;
using DaveSexton.Labs;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Reactive Trace")]
	[Description("Example usage of the IObservable<T> Trace* extensions.")]
	public sealed class TraceLab : RxxLab
	{
		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			System.Diagnostics.Trace.Listeners.Add(
				new AnonymousTraceListener(Lab.Trace, Lab.TraceLine));

			var xs = Observable
				.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
				.TraceOnNext(value => "OnNext: " + value)
				.TraceSubscriptions();

			var query = Observable
				.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(3))
				.Select(_ => xs)
				.Switch();

			using (query.Subscribe())
			{
				Console.ReadKey();
			}

			System.Diagnostics.Trace.Listeners.Clear();
		}
	}
}
