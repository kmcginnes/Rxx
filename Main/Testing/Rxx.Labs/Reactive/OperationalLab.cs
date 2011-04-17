using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DaveSexton.Labs;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Operator Overloads")]
	[Description("Reactive AsOperational extensions.")]
	public sealed class OperationalLab : RxxLab
	{
		protected override void Main()
		{
			TraceDescription(Text.OperationalLabBasic);
			TraceLine(Instructions.PressAnyKeyToCancel);

			var xs = new Subject<int>();
			var ys = new Subject<int>();
			var zs = new Subject<int>();

			var oxs = xs.AsOperational();

			var query = oxs + ys - zs;

			using (query.Subscribe(ConsoleOutput))
			{
				xs.OnNext(1);
				ys.OnNext(5);
				zs.OnNext(2);

				xs.OnNext(2);
				ys.OnNext(6);
				zs.OnNext(3);

				xs.OnNext(3);
				xs.OnNext(4);

				ys.OnNext(7);
				ys.OnNext(8);

				zs.OnNext(4);
				zs.OnNext(5);

				Console.ReadKey();
			}

			TraceLine();
			TraceDescription(Text.OperationalLabAdvanced);
			TraceLine(Instructions.PressAnyKeyToCancel);

			// Define an operational factory so operators are only specified once.
			var o = (Func<IObservable<int>, OperationalObservable<int>>)
				(source => source.AsOperational(
					add: (left, right) => left + (right * 5),
					subtract: (left, right) => left - right,
					multiply: (left, right) => left * right,
					divide: (left, right) => (left * 2) / right,
					negative: value => -value));

			var query2 = (-o(xs) * 2) + ys - (o(zs) / 4);

			using (query2.Subscribe(ConsoleOutput))
			{
				xs.OnNext(1);
				ys.OnNext(5);
				zs.OnNext(4);

				xs.OnNext(2);
				ys.OnNext(6);
				zs.OnNext(8);

				xs.OnNext(3);
				ys.OnNext(7);
				zs.OnNext(12);

				Console.ReadKey();
			}
		}
	}
}