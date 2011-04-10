using System.Diagnostics.Contracts;
using System.Linq;

namespace System
{
	public static class PairedObservableExtensions
	{
		public static IDisposable Subscribe<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight)
		{
			Contract.Requires(source != null);
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			return source.Subscribe(PairedObserver.Create(
				onNextLeft,
				onNextRight));
		}

		public static IDisposable Subscribe<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError)
		{
			Contract.Requires(source != null);
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			return source.Subscribe(PairedObserver.Create(
				onNextLeft,
				onNextRight,
				onError));
		}

		public static IDisposable Subscribe<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action onCompleted)
		{
			Contract.Requires(source != null);
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			return source.Subscribe(PairedObserver.Create(
				onNextLeft,
				onNextRight,
				onCompleted));
		}

		public static IDisposable Subscribe<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			Action<TLeft> onNextLeft,
			Action<TRight> onNextRight,
			Action<Exception> onError,
			Action onCompleted)
		{
			Contract.Requires(source != null);
			Contract.Requires(onNextLeft != null);
			Contract.Requires(onNextRight != null);
			Contract.Requires(onError != null);
			Contract.Requires(onCompleted != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			return source.Subscribe(PairedObserver.Create(
				onNextLeft,
				onNextRight,
				onError,
				onCompleted));
		}
	}
}
