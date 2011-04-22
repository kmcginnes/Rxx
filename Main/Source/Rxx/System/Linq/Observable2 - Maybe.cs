using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<Maybe<TSource>> Maybe<TSource>(this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<Maybe<TSource>>>() != null);

			var observable = Observable.CreateWithDisposable<Maybe<TSource>>(
				observer =>
				{
					observer.OnNext(System.Maybe<TSource>.Empty);

					return source.Subscribe(
						value => observer.OnNext(new Maybe<TSource>(value)),
						observer.OnError,
						observer.OnCompleted);
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}