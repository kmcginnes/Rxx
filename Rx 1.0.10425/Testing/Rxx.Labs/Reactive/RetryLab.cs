using System;
using System.ComponentModel;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Retry")]
	[Description("Pairing a faulty sequence with an OnError sequence.")]
	public sealed class RetryLab : RxxLab
	{
		private static int e;

		private IObservable<int> xs = Observable.If(
			() => e < 6,
			thenSource: Observable.Defer(() => Observable
				.Return(e)
				.Concat(Observable.Throw<int>(new Exception("Error " + e++)))),
			elseSource: Observable.Defer(() => Observable
				.Throw<int>(new Exception("Consecutive Error " + e++))));

		private void RetryExperiment()
		{
			e = 1;
			var paired = xs.Retry<int, Exception>(5);

			paired.Run(
				ConsoleOutputOnNext<int>(),
				ConsoleOutputOnNext<Exception>(ex => ex.Message),
				ConsoleOutputOnError());
		}

		private void RetryConsecutiveExperiment()
		{
			e = 1;
			var consecutive = xs.RetryConsecutive(3);

			consecutive.Run(
				ConsoleOutputOnNext<int>(),
				ConsoleOutputOnNext<Exception>(ex => ex.Message),
				ConsoleOutputOnError());
		}

		private void RetryLinearBackOffExperiment()
		{
			e = 1;
			var linearBackOff = xs.Retry(
				retryCount: 5,
				backOffSelector: (ex, attemptCount) => TimeSpan.FromSeconds(.5 * attemptCount));

			linearBackOff.Run(
				ConsoleOutputOnNext<int>(),
				ConsoleOutputOnNext<Exception>(ex => ex.Message),
				ConsoleOutputOnError());
		}

		private void RetryExponentialBackOffExperiment()
		{
			e = 1;
			var exponentialBackOff = xs.Retry(
				retryCount: 5,
				backOffSelector: (ex, attemptCount) => TimeSpan.FromSeconds(.5 * Math.Pow(2, attemptCount - 1)));

			exponentialBackOff.Run(
				ConsoleOutputOnNext<int>(),
				ConsoleOutputOnNext<Exception>(ex => ex.Message),
				ConsoleOutputOnError());
		}

		protected override void Main()
		{
			TraceLine(Instructions.WaitForError);

			RetryExperiment();

			PressAnyKeyToContinue();

			TraceLine();
			TraceLine(Instructions.WaitForError);

			RetryConsecutiveExperiment();

			PressAnyKeyToContinue();

			TraceLine();
			TraceLine(Instructions.WaitForError);

			RetryLinearBackOffExperiment();

			PressAnyKeyToContinue();

			TraceLine();
			TraceLine(Instructions.WaitForError);

			RetryExponentialBackOffExperiment();
		}
	}
}
