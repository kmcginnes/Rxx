using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
	public class TraceObserver<T> : IObserver<T>, IObserver<T, string>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly Func<T, string> onNext;
		private readonly Func<Exception, string> onError;
		private readonly Func<string> onCompleted;
		private readonly TraceSource trace;
		#endregion

		#region Constructors
		public TraceObserver()
			: this(TraceDefaults.DefaultOnNext, TraceDefaults.DefaultOnError, TraceDefaults.DefaultOnCompleted)
		{
		}

		public TraceObserver(Func<T, string> onNext)
		{
			Contract.Requires(onNext != null);

			this.onNext = onNext;
		}

		public TraceObserver(Func<T, string> onNext, Func<Exception, string> onError)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			this.onError = onError;
		}

		public TraceObserver(Func<T, string> onNext, Func<string> onCompleted)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			this.onCompleted = onCompleted;
		}

		public TraceObserver(Func<T, string> onNext, Func<Exception, string> onError, Func<string> onCompleted)
			: this(onNext, onError)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);

			this.onCompleted = onCompleted;
		}

		public TraceObserver(string nextFormat)
			: this(TraceDefaults.GetFormatOnNext<T>(nextFormat))
		{
			Contract.Requires(nextFormat != null);
		}

		public TraceObserver(string nextFormat, string errorFormat)
			: this(TraceDefaults.GetFormatOnNext<T>(nextFormat), TraceDefaults.GetFormatOnError(errorFormat))
		{
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
		}

		public TraceObserver(string nextFormat, string errorFormat, string completedMessage)
			: this(TraceDefaults.GetFormatOnNext<T>(nextFormat), TraceDefaults.GetFormatOnError(errorFormat), TraceDefaults.GetMessageOnCompleted(completedMessage))
		{
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
			Contract.Requires(completedMessage != null);
		}

		public TraceObserver(TraceSource trace)
			: this(trace, TraceDefaults.DefaultOnNext, TraceDefaults.DefaultOnError, TraceDefaults.DefaultOnCompleted)
		{
			Contract.Requires(trace != null);
		}

		public TraceObserver(TraceSource trace, Func<T, string> onNext)
			: this(onNext)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, Func<T, string> onNext, Func<Exception, string> onError)
			: this(onNext, onError)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, Func<T, string> onNext, Func<string> onCompleted)
			: this(onNext, onCompleted)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, Func<T, string> onNext, Func<Exception, string> onError, Func<string> onCompleted)
			: this(onNext, onError, onCompleted)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, string nextFormat)
			: this(nextFormat)
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, string nextFormat, string errorFormat)
			: this(nextFormat, errorFormat)
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);

			this.trace = trace;
		}

		public TraceObserver(TraceSource trace, string nextFormat, string errorFormat, string completedMessage)
			: this(nextFormat, errorFormat, completedMessage)
		{
			Contract.Requires(trace != null);
			Contract.Requires(nextFormat != null);
			Contract.Requires(errorFormat != null);
			Contract.Requires(completedMessage != null);

			this.trace = trace;
		}

		public TraceObserver(IObserver<T, string> formattingObserver)
		{
			Contract.Requires(formattingObserver != null);

			this.onNext = formattingObserver.OnNext;
			this.onError = formattingObserver.OnError;
			this.onCompleted = formattingObserver.OnCompleted;
		}

		public TraceObserver(TraceSource trace, IObserver<T, string> formattingObserver)
			: this(formattingObserver)
		{
			Contract.Requires(trace != null);
			Contract.Requires(formattingObserver != null);

			this.trace = trace;
		}
		#endregion

		#region Methods
		protected virtual string FormatOnNext(T value)
		{
			if (onNext != null)
				return onNext(value);
			else
				return null;
		}

		protected virtual string FormatOnError(Exception exception)
		{
			if (onError != null && exception != null)
				return onError(exception);
			else
				return null;
		}

		protected virtual string FormatOnCompleted()
		{
			if (onCompleted != null)
				return onCompleted();
			else
				return null;
		}
		#endregion

		#region IObserver<T>
		public void OnNext(T value)
		{
			string message = FormatOnNext(value);

			if (message != null)
			{
				if (trace != null)
					trace.TraceInformation(message);
				else
					Trace.TraceInformation(message);
			}
		}

		public void OnError(Exception error)
		{
			string message = FormatOnError(error);

			if (message != null)
			{
				if (trace != null)
					trace.TraceEvent(TraceEventType.Error, 0, message);
				else
					Trace.TraceError(message);
			}
		}

		public void OnCompleted()
		{
			string message = FormatOnCompleted();

			if (message != null)
			{
				if (trace != null)
					trace.TraceInformation(message);
				else
					Trace.TraceInformation(message);
			}
		}
		#endregion

		#region IObserver<T,string> Members
		string IObserver<T, string>.OnNext(T value)
		{
			return FormatOnNext(value);
		}

		string IObserver<T, string>.OnError(Exception exception)
		{
			return FormatOnError(exception);
		}

		string IObserver<T, string>.OnCompleted()
		{
			return FormatOnCompleted();
		}
		#endregion
	}
}
