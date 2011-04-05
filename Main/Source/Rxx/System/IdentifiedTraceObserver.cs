using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

namespace System
{
	public class IdentifiedTraceObserver<T> : TraceObserver<T>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private static int counter;

		private readonly int id;
		private readonly Func<int, T, string> onNext;
		private readonly Func<int, Exception, string> onError;
		private readonly Func<int, string> onCompleted;
		#endregion

		#region Constructors
		public IdentifiedTraceObserver()
			: this(TraceDefaults.DefaultOnNext, TraceDefaults.DefaultOnError, TraceDefaults.DefaultOnCompleted)
		{
		}

		public IdentifiedTraceObserver(Func<int, T, string> onNext)
		{
			Contract.Requires(onNext != null);

			id = Interlocked.Increment(ref counter);

			this.onNext = onNext;
		}

		public IdentifiedTraceObserver(Func<int, T, string> onNext, Func<int, Exception, string> onError)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			this.onError = onError;
		}

		public IdentifiedTraceObserver(Func<int, T, string> onNext, Func<int, string> onCompleted)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(Func<int, T, string> onNext, Func<int, Exception, string> onError, Func<int, string> onCompleted)
			: this(onNext, onError)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);

			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(string nextFormat)
			: this(TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat))
		{
			Contract.Requires(nextFormat != null);
		}

		public IdentifiedTraceObserver(string nextFormat, string errorFormat)
			: this(TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat), TraceDefaults.GetIdentityFormatOnError(errorFormat))
		{
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
		}

		public IdentifiedTraceObserver(string nextFormat, string errorFormat, string completedMessage)
			: this(TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat), TraceDefaults.GetIdentityFormatOnError(errorFormat), TraceDefaults.GetIdentityMessageOnCompleted(completedMessage))
		{
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
			Contract.Requires(completedMessage != null);
		}

		public IdentifiedTraceObserver(TraceSource trace)
			: this(trace, TraceDefaults.DefaultOnNext, TraceDefaults.DefaultOnError, TraceDefaults.DefaultOnCompleted)
		{
			Contract.Requires(trace != null);
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<int, T, string> onNext)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);

			id = Interlocked.Increment(ref counter);

			this.onNext = onNext;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<int, T, string> onNext, Func<int, Exception, string> onError)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			id = Interlocked.Increment(ref counter);

			this.onNext = onNext;
			this.onError = onError;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<int, T, string> onNext, Func<int, string> onCompleted)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			id = Interlocked.Increment(ref counter);

			this.onNext = onNext;
			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<int, T, string> onNext, Func<int, Exception, string> onError, Func<int, string> onCompleted)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);

			id = Interlocked.Increment(ref counter);

			this.onNext = onNext;
			this.onError = onError;
			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(TraceSource trace, string nextFormat)
			: this(trace, TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat))
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);
		}

		public IdentifiedTraceObserver(TraceSource trace, string nextFormat, string errorFormat)
			: this(trace, TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat), TraceDefaults.GetIdentityFormatOnError(errorFormat))
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
		}

		public IdentifiedTraceObserver(TraceSource trace, string nextFormat, string errorFormat, string completedMessage)
			: this(trace, TraceDefaults.GetIdentityFormatOnNext<T>(nextFormat), TraceDefaults.GetIdentityFormatOnError(errorFormat), TraceDefaults.GetIdentityMessageOnCompleted(completedMessage))
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
			Contract.Requires(completedMessage != null);
		}
		#endregion

		#region Methods
		protected sealed override string FormatOnNext(T value)
		{
			return FormatOnNext(id, value);
		}

		protected sealed override string FormatOnError(Exception exception)
		{
			return FormatOnError(id, exception);
		}

		protected sealed override string FormatOnCompleted()
		{
			return FormatOnCompleted(id);
		}

		protected virtual string FormatOnNext(int observerId, T value)
		{
			if (onNext != null)
				return onNext(observerId, value);
			else
				return null;
		}

		protected virtual string FormatOnError(int observerId, Exception exception)
		{
			if (onError != null)
				return onError(observerId, exception);
			else
				return null;
		}

		protected virtual string FormatOnCompleted(int observerId)
		{
			if (onCompleted != null)
				return onCompleted(observerId);
			else
				return null;
		}
		#endregion
	}
}
