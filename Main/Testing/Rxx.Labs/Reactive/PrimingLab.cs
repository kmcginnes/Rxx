using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Primed Observables")]
	[Description("Using the Prime and PrimeStart operators.")]
	public sealed class PrimingLab : RxxLab
	{
		protected override void Main()
		{
			TraceDescription(Text.PrimingLabPrime);
			TraceLine(Instructions.WaitForCompletion);

			var xs = Observable.Interval(TimeSpan.FromSeconds(1))
				.Do(ConsoleOutput(Text.Generated))
				.Take(3);

			IObservable<long> pruned = xs.Prune().Prime();

			for (int r = 0; r < 2; r++)
			{
				for (int s = 0; s < 5; s++)
				{
					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(.4));

					TraceLine(Text.SubscribingFormat, s);

					pruned.Subscribe(ConsoleOutput(Text.NamedObserverFormat, s));
				}

				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
			}

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToContinue);

			Console.ReadKey();

			TraceLine();
			TraceDescription(Text.PrimingLabStartPrimed);
			TraceLine(Instructions.WaitForCompletion);

			int count = 0;
			var ys = Observable2.StartPrimed(() => count++);

			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

			TraceLine(Text.StartedFormat, count > 0);
			TraceLine(Text.Subscribing);

			ys.Subscribe(ConsoleOutput);

			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

			TraceLine(Text.StartedFormat, count > 0);

			ys.Subscribe(ConsoleOutput);

			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

			TraceLine(Text.GeneratedFormat, count > 1);
		}
	}
}
