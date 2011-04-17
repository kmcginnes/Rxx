using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Interactive
{
	[DisplayName("Operator Overloads")]
	[Description("Interactive AsOperational extensions.")]
	public sealed class OperationalLab : RxxLab
	{
		protected override void Main()
		{
			TraceDescription(Text.OperationalLabBasic);

			var xs = new[] { 1, 2, 3, 4 };
			var ys = new[] { 5, 6, 7, 8 };
			var zs = new[] { 2, 3, 4, 5 };

			var oxs = xs.AsOperational();

			var query = oxs + ys - zs;

			query.Run(ConsoleOutput);

			TraceLine();
			TraceLine(Instructions.PressAnyKeyToContinue);

			Console.ReadKey();

			TraceLine();
			TraceDescription(Text.OperationalLabAdvanced);

			// Define an operational factory so operators are only specified once.
			var o = (Func<IEnumerable<int>, OperationalEnumerable<int>>)
				(source => source.AsOperational(
					add: (left, right) => left + (right * 5),
					subtract: (left, right) => left - right,
					multiply: (left, right) => left * right,
					divide: (left, right) => (left * 2) / right,
					negative: value => -value));

			xs = new[] { 1, 2, 3 };
			ys = new[] { 5, 6, 7 };
			zs = new[] { 4, 8, 12 };

			var query2 = (-o(xs) * 2) + ys - (o(zs) / 4);

			query2.Run(ConsoleOutput);
		}
	}
}