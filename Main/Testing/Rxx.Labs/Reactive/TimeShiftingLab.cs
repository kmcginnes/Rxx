using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Time Shifting")]
	[Description("Converting sequences of arbitrary data to Intervals, Timers and Pulses.")]
	public sealed class TimeShiftingLab : RxxLab
	{
		private void IntervalExperiment()
		{
			var xs = Observable.Range(1, 3)
				.Concat(Observable2.Delay(-1, TimeSpan.FromSeconds(4.5)).IgnoreValues())
				.Concat(Observable.Range(4, 3))
				.AsInterval(TimeSpan.FromSeconds(1));

			using (xs.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}
		}

		private void TimerExperiment()
		{
			var xs = Observable.Range(1, 3)
				.Concat(Observable2.Delay(-1, TimeSpan.FromSeconds(4.5)).IgnoreValues())
				.Concat(Observable.Range(4, 3))
				.AsTimer(TimeSpan.FromSeconds(1));

			using (xs.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}
		}

		private void PulseExperiment()
		{
			var xs = Observable.Range(1, 3)
				.Concat(Observable2.Delay(-1, TimeSpan.FromSeconds(4.5)).IgnoreValues())
				.Concat(Observable.Range(4, 3))
				.Pulse(TimeSpan.FromSeconds(1));

			using (xs.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}
		}

		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			IntervalExperiment();

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToCancel);

			TimerExperiment();

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToCancel);

			PulseExperiment();
		}
	}
}
