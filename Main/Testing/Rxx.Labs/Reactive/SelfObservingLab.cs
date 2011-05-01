using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Self-Observations")]
	[Description("Buffering while observing and also pairing an observable with introspection.")]
	public sealed class SelfObservingLab : RxxLab
	{
		private void DurationExperiment()
		{
			var xs = Observable
				.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
				.Take(4)
				.Introspect();

			xs.Run(
				durationWindow =>
				{
					durationWindow.Subscribe(
						ConsoleOutputOnNext<long>(Text.Duration),
						ConsoleOutputOnError(Text.Duration),
						ConsoleOutputOnCompleted(Text.Duration));
				},
				ConsoleOutputOnNext<long>(
					value =>
					{
						Thread.Sleep(TimeSpan.FromSeconds(2));

						return value.ToString(CultureInfo.CurrentCulture);
					}),
				ConsoleOutputOnError(),
				ConsoleOutputOnCompleted());
		}

		private void BufferExperiment()
		{
			var xs = Observable
				.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
				.Take(10)
				.BufferIntrospective();

			xs.Run(
				ConsoleOutputOnNext<IList<long>>(
					values =>
					{
						TraceLine(Text.ObservingFormat, values.Count);

						Thread.Sleep(TimeSpan.FromSeconds(2.5));

						return values.Aggregate(
							new System.Text.StringBuilder(),
							(acc, cur) => acc.Append(cur).Append(','),
							acc => acc.ToString(0, Math.Max(0, acc.Length - 1)));
					}),
				ConsoleOutputOnError(),
				ConsoleOutputOnCompleted());
		}

		protected override void Main()
		{
			TraceLine(Instructions.WaitForCompletion);

			DurationExperiment();

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToContinue);

			Console.ReadKey();

			TraceLine();
			TraceLine(Instructions.WaitForCompletion);

			BufferExperiment();
		}
	}
}
