using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		public static IObservable<TLeft> TakeLeft<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TLeft>>() != null);

			var observable = Observable.CreateWithDisposable<TLeft>(
				observer =>
				{
					return source.Subscribe(
						observer.OnNext,
						right => { },
						observer.OnError,
						observer.OnCompleted);
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<TRight> TakeRight<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TRight>>() != null);

			var observable = Observable.CreateWithDisposable<TRight>(
				observer =>
				{
					return source.Subscribe(
						left => { },
						observer.OnNext,
						observer.OnError,
						observer.OnCompleted);
				});

			Contract.Assume(observable != null);

			return observable;
		}

		public static IPairedObservable<TLeft, TRight> TakeLeft<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			int count)
		{
			Contract.Requires(source != null);
			Contract.Requires(count >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					int remaining = count;

					return source.Subscribe(
						left =>
						{
							if (remaining > 0)
							{
								remaining--;

								observer.OnNextLeft(left);

								if (remaining == 0)
								{
									observer.OnCompleted();
								}
							}
						},
						observer.OnNextRight,
						observer.OnError,
						observer.OnCompleted);
				});
		}

		public static IPairedObservable<TLeft, TRight> TakeRight<TLeft, TRight>(
			this IObservable<Either<TLeft, TRight>> source,
			int count)
		{
			Contract.Requires(source != null);
			Contract.Requires(count >= 0);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return PairedObservable.CreateWithDisposable<TLeft, TRight>(
				observer =>
				{
					int remaining = count;

					return source.Subscribe(
						observer.OnNextLeft,
						right =>
						{
							if (remaining > 0)
							{
								remaining--;

								observer.OnNextRight(right);

								if (remaining == 0)
								{
									observer.OnCompleted();
								}
							}
						},
						observer.OnError,
						observer.OnCompleted);
				});
		}
	}
}
