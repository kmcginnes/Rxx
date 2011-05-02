using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("How To Create a Lab")]
	[Description("This is an example for developers on how to properly write a reactive lab.")]
	public sealed class ExampleLab : RxxLab
	{
		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			var xs = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

			using (xs.Subscribe(ConsoleOutput))
			{
				Console.ReadKey();
			}
		}
	}
}