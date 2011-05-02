using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Paired Observables")]
	[Description("Tracking progress with IPairedObservable and the Pair operator.")]
	public sealed class PairingLab : RxxLab
	{
		private void ValueDependentExperiment()
		{
			var source = Observable.Interval(TimeSpan.FromSeconds(.5))
				.Select(v => (double) ++v)
				.Take(10);

			var sourceWithProgress = source.Pair(value => value * 10 / 100).Publish();
			var progressOnly = sourceWithProgress.TakeRight();
			var resultOnly = sourceWithProgress.TakeLeft().TakeLast(1);

			using (progressOnly.Subscribe(ConsoleOutputFormat(Text.Progress, "{0,5:P0}")))
			using (resultOnly.Subscribe(ConsoleOutput(Text.Result)))
			using (sourceWithProgress.Connect())
			{
				Console.ReadKey();
			}
		}

		private void TimeDependentExperiment()
		{
			var progressByTime = Observable.Interval(TimeSpan.FromSeconds(.5))
				.Select(v => (double) ++v * 10 / 100)
				.Take(10)
				.Publish()
				.Prime();

			var source = Observable.Interval(TimeSpan.FromSeconds(.2))
				.TakeUntil(progressByTime.Where(progress => progress == 1));

			var sourceWithProgress = source.Pair(progressByTime).Publish();
			var progressOnly = sourceWithProgress.TakeRight().DistinctUntilChanged();
			var resultOnly = sourceWithProgress.TakeLeft().TakeLast(1);

			using (progressOnly.Subscribe(ConsoleOutputFormat(Text.Progress, "{0,5:P0}")))
			using (resultOnly.Subscribe(ConsoleOutput(Text.Result)))
			using (sourceWithProgress.Connect())
			{
				Console.ReadKey();
			}
		}

		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			ValueDependentExperiment();

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToCancel);

			TimeDependentExperiment();
		}
	}
}
