using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Time Shifting")]
	[Description("Creating Interval and Timer sequences with arbitrary data.")]
	public sealed class TimeShiftingLab : RxxLab
	{
		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			var xs = Observable.Range(1, 5).AsInterval(TimeSpan.FromSeconds(1));

			using (xs.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToCancel);

			var ys = Observable.Range(1, 5).AsTimer(TimeSpan.FromSeconds(1));

			using (ys.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}
		}
	}
}
