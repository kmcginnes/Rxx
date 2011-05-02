using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Disposables;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		/// <summary>
		/// Creates a paired observable sequence from the <paramref name="subscribe"/> implementation.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="subscribe">Subscribes observers to the paired observable.</param>
		/// <returns>A paired observable that calls the specified <paramref name="subscribe"/> function when an observer subscribes.</returns>
		public static IPairedObservable<TLeft, TRight> Create<TLeft, TRight>(
			Func<IPairedObserver<TLeft, TRight>, Action> subscribe)
		{
			Contract.Requires(subscribe != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			return CreateWithDisposable<TLeft, TRight>(o => Disposable.Create(subscribe(o)));
		}

		/// <summary>
		/// Creates a paired observable sequence from the <paramref name="subscribe"/> implementation.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <param name="subscribe">Subscribes observers to the paired observable.</param>
		/// <returns>A paired observable that calls the specified <paramref name="subscribe"/> function when an observer subscribes.</returns>
		public static IPairedObservable<TLeft, TRight> CreateWithDisposable<TLeft, TRight>(
			Func<IPairedObserver<TLeft, TRight>, IDisposable> subscribe)
		{
			Contract.Requires(subscribe != null);
			Contract.Ensures(Contract.Result<IPairedObservable<TLeft, TRight>>() != null);

			var observable = Observable.CreateWithDisposable<Either<TLeft, TRight>>(
				observer =>
				{
					return subscribe(new AnonymousPairedObserver<TLeft, TRight>(observer));
				});

			Contract.Assume(observable != null);

			return new AnonymousPairedObservable<TLeft, TRight>(observable);
		}
	}
}
