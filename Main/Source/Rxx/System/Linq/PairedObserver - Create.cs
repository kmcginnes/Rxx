using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static class PairedObserver
	{
		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				ex =>
				{
					throw ex.PrepareForRethrow();
				},
				() => { });
		}

		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				onError,
				() => { });
		}

		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action onCompleted)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			return Create(
				onNextLeft,
				onNextRight,
				ex =>
				{
					throw ex.PrepareForRethrow();
				},
				onCompleted);
		}

		public static IPairedObserver<TLeft, TRight> Create<TLeft, TRight>(
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError,
			Action onCompleted)
		{
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IPairedObserver<TLeft, TRight>>() != null);

			var observer = Observer.Create<Either<TLeft, TRight>>(
				value => value.Switch(onNextLeft, onNextRight),
				onError,
				onCompleted);

			Contract.Assume(observer != null);

			return new AnonymousPairedObserver<TLeft, TRight>(observer);
		}
	}
}
