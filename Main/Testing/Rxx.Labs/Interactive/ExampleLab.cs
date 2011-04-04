using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Interactive
{
	[DisplayName("How To Create a Lab")]
	[Description("This is an example for developers on how to properly write an interactive lab.")]
	public sealed class ExampleLab : RxxLab
	{
		protected override void Main()
		{
			var xs = Enumerable.Range(0, 5);

			TraceLine(Instructions.WaitForCompletion);

			xs.Run(ConsoleOutput);
		}
	}
}