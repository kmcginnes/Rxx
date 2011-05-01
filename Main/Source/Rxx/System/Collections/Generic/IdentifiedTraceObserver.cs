using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

namespace System.Collections.Generic
{
	public class IdentifiedTraceObserver<T> : TraceObserver<T>
	{
		#region Public Properties
		public string Identity
		{
			get
			{
				Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

				return id;
			}
			set
			{
				Contract.Requires(!string.IsNullOrWhiteSpace(value));

				this.id = value;
			}
		}
		#endregion

		#region Private / Protected
		private string id;
		private readonly Func<string, T, string> onNext;
		private readonly Func<string, Exception, string> onError;
		private readonly Func<string, string> onCompleted;
		#endregion

		#region Constructors
		public IdentifiedTraceObserver()
			: this(TraceDefaults.DefaultOnNext, TraceDefaults.DefaultOnError, TraceDefaults.DefaultOnCompleted)
		{
		}

		public IdentifiedTraceObserver(Func<string, T, string> onNext)
		{
			Contract.Requires(onNext != null);

			id = AutoIdentify();

			this.onNext = onNext;
		}

		public IdentifiedTraceObserver(Func<string, T, string> onNext, Func<string, Exception, string> onError)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			this.onError = onError;
		}

		public IdentifiedTraceObserver(Func<string, T, string> onNext, Func<string, string> onCompleted)
			: this(onNext)
		{
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(Func<string, T, string> onNext, Func<string, Exception, string> onError, Func<string, string> onCompleted)
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

		public IdentifiedTraceObserver(TraceSource trace, Func<string, T, string> onNext)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);

			id = AutoIdentify();

			this.onNext = onNext;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<string, T, string> onNext, Func<string, Exception, string> onError)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);

			id = AutoIdentify();

			this.onNext = onNext;
			this.onError = onError;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<string, T, string> onNext, Func<string, string> onCompleted)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onCompleted != null);

			id = AutoIdentify();

			this.onNext = onNext;
			this.onCompleted = onCompleted;
		}

		public IdentifiedTraceObserver(TraceSource trace, Func<string, T, string> onNext, Func<string, Exception, string> onError, Func<string, string> onCompleted)
			: base(trace)
		{
			Contract.Requires(trace != null);
			Contract.Requires(onNext != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);

			id = AutoIdentify();

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
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(!string.IsNullOrWhiteSpace(id));
		}

		private static string AutoIdentify()
		{
			Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

			var identity = Interlocked.Increment(ref TraceDefaults.IdentityCounter)
				.ToString(System.Globalization.CultureInfo.InvariantCulture);

			Contract.Assume(!string.IsNullOrWhiteSpace(identity));

			return identity;
		}

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

		protected virtual string FormatOnNext(string observerId, T value)
		{
			if (onNext != null)
				return onNext(observerId, value);
			else
				return null;
		}

		protected virtual string FormatOnError(string observerId, Exception exception)
		{
			if (onError != null)
				return onError(observerId, exception);
			else
				return null;
		}

		protected virtual string FormatOnCompleted(string observerId)
		{
			if (onCompleted != null)
				return onCompleted(observerId);
			else
				return null;
		}
		#endregion
	}
}
