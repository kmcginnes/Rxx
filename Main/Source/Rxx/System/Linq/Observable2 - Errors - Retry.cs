using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IPairedObservable<TSource, TException> Retry<TSource, TException>(
			this IObservable<TSource> source,
			int retryCount)
			where TException : Exception
		{
			Contract.Requires(source != null);
			Contract.Requires(retryCount >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return Retry<TSource, TException>(source, retryCount, (ex, i) => TimeSpan.Zero);
		}

		public static IPairedObservable<TSource, Exception> Retry<TSource>(
			this IObservable<TSource> source,
			int retryCount,
			Func<Exception, int, TimeSpan> backOffSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(retryCount >= 0);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, Exception>>() != null);

			return Retry<TSource, Exception>(source, retryCount, backOffSelector);
		}

		public static IPairedObservable<TSource, TException> Retry<TSource, TException>(
			this IObservable<TSource> source,
			int retryCount,
			Func<TException, int, TimeSpan> backOffSelector)
			where TException : Exception
		{
			Contract.Requires(source != null);
			Contract.Requires(retryCount >= 0);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			var observable = Observable.Defer<Either<TSource, TException>>(() =>
				{
					int attemptCount = 1;

					var sources = Enumerable.Repeat(source, retryCount).GetEnumerator();

					return sources.Catch<TSource, TException>(
						ex => sources,
						ex => backOffSelector(ex, attemptCount++));
				});

			Contract.Assume(observable != null);

			return observable.AsPairedObservable();
		}

		/// <remarks>
		/// <see cref="RetryConsecutive"/> is appropriate when permanent recovery is required for sequences 
		/// that experience ephemeral consecutive errors at unpredictable intervals, such as those originating 
		/// from network streams.  For example, it can produce a sequence that automatically reconnects upon 
		/// consecutive network failures up to the specified <paramref name="consecutiveRetryCount"/> number of 
		/// times; furthermore, if the sequence is able to successfully generate a value after an error, then 
		/// the retry count is reset for subsequent consecutive failures.
		/// </remarks>
		public static IPairedObservable<TSource, Exception> RetryConsecutive<TSource>(
			this IObservable<TSource> source,
			int consecutiveRetryCount)
		{
			Contract.Requires(source != null);
			Contract.Requires(consecutiveRetryCount >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, Exception>>() != null);

			return RetryConsecutive<TSource, Exception>(source, consecutiveRetryCount, (ex, i) => TimeSpan.Zero);
		}

		/// <remarks>
		/// <see cref="RetryConsecutive"/> is appropriate when permanent recovery is required for sequences 
		/// that experience ephemeral consecutive errors at unpredictable intervals, such as those originating 
		/// from network streams.  For example, it can produce a sequence that automatically reconnects upon 
		/// consecutive network failures up to the specified <paramref name="consecutiveRetryCount"/> number of 
		/// times; furthermore, if the sequence is able to successfully generate a value after an error, then 
		/// the retry count is reset for subsequent consecutive failures.
		/// </remarks>
		public static IPairedObservable<TSource, TException> RetryConsecutive<TSource, TException>(
			this IObservable<TSource> source,
			int consecutiveRetryCount)
			where TException : Exception
		{
			Contract.Requires(source != null);
			Contract.Requires(consecutiveRetryCount >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return RetryConsecutive<TSource, TException>(source, consecutiveRetryCount, (ex, i) => TimeSpan.Zero);
		}

		/// <remarks>
		/// <see cref="RetryConsecutive"/> is appropriate when permanent recovery is required for sequences 
		/// that experience ephemeral consecutive errors at unpredictable intervals, such as those originating 
		/// from network streams.  For example, it can produce a sequence that automatically reconnects upon 
		/// consecutive network failures up to the specified <paramref name="consecutiveRetryCount"/> number of 
		/// times; furthermore, if the sequence is able to successfully generate a value after an error, then 
		/// the retry count is reset for subsequent consecutive failures.
		/// </remarks>
		/// <seealso href="http://en.wikipedia.org/wiki/Exponential_backoff">
		/// Exponential backoff
		/// </seealso>
		public static IPairedObservable<TSource, Exception> RetryConsecutive<TSource>(
			this IObservable<TSource> source,
			int consecutiveRetryCount,
			Func<Exception, int, TimeSpan> backOffSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(consecutiveRetryCount >= 0);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, Exception>>() != null);

			return RetryConsecutive<TSource, Exception>(source, consecutiveRetryCount, backOffSelector);
		}

		/// <remarks>
		/// <see cref="RetryConsecutive"/> is appropriate when permanent recovery is required for sequences 
		/// that experience ephemeral consecutive errors at unpredictable intervals, such as those originating 
		/// from network streams.  For example, it can produce a sequence that automatically reconnects upon 
		/// consecutive network failures up to the specified <paramref name="consecutiveRetryCount"/> number of 
		/// times; furthermore, if the sequence is able to successfully generate a value after an error, then 
		/// the retry count is reset for subsequent consecutive failures.
		/// </remarks>
		/// <seealso href="http://en.wikipedia.org/wiki/Exponential_backoff">
		/// Exponential backoff
		/// </seealso>
		public static IPairedObservable<TSource, TException> RetryConsecutive<TSource, TException>(
			this IObservable<TSource> source,
			int consecutiveRetryCount,
			Func<TException, int, TimeSpan> backOffSelector)
			where TException : Exception
		{
			Contract.Requires(source != null);
			Contract.Requires(consecutiveRetryCount >= 0);
			Contract.Requires(backOffSelector != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TSource, TException>>() != null);

			return PairedObservable.CreateWithDisposable<TSource, TException>(
				observer =>
				{
					int attemptCount = 1;
					bool decremented = false;
					bool resetRequired = false;

					var sources = Enumerable.Repeat(source, consecutiveRetryCount).GetEnumerator();

					return sources
						.Catch<TSource, TException>(
							ex =>
							{
								if (resetRequired)
								{
									if (!decremented)
									/* This behavior matches the Rx behavior of the retryCount parameter in the Retry method.
										* The first iteration always counts as the first "retry", even though technically it's 
										* not a "retry" because it's first.  (If consecutiveRetryCount is set to zero, then the 
										* sequence will end because the enumerator's MoveNext method called below will return false.)
										*/
									{
										Contract.Assume(consecutiveRetryCount > 0);

										consecutiveRetryCount--;
										decremented = true;
									}

									attemptCount = 1;
									resetRequired = false;

									sources = Enumerable.Repeat(source, consecutiveRetryCount).GetEnumerator();
								}

								return sources;
							},
							ex => backOffSelector(ex, attemptCount++))
						.Subscribe(
							value =>
							{
								resetRequired = true;

								observer.OnNextLeft(value);
							},
							observer.OnNextRight,
							observer.OnError,
							observer.OnCompleted);
				});
		}
	}
}
