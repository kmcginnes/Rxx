using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DaveSexton.Labs;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Interactive
{
	[DisplayName("Interactive Trace")]
	[Description("Example usage of the IEnumerable<T> Trace* extensions.")]
	public sealed class TraceLab : RxxLab
	{
		protected override void Main()
		{
			TraceLine(Instructions.IxTraceLabInstructions);

			System.Diagnostics.Trace.Listeners.Add(
				new AnonymousTraceListener(Lab.Trace, Lab.TraceLine));

			var lines = GetLinesFromUser()
				.TraceOnNext(value => "OnNext: " + value)
				.TraceOnCompleted(Text.Done);

			lines.Run();

			System.Diagnostics.Trace.Listeners.Clear();
		}

		private static IEnumerable<string> GetLinesFromUser()
		{
			Console.WriteLine();

			do
			{
				Console.Write(Text.PromptFormat, Instructions.Input);

				string line = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(line))
					break;

				yield return line;
			}
			while (true);
		}
	}
}
