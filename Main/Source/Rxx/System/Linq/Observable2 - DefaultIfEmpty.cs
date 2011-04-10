using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		public static IObservable<TSource> DefaultIfEmpty<TSource>(
			this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return DefaultIfEmpty(source, default(TSource));
		}

		public static IObservable<TSource> DefaultIfEmpty<TSource>(
			this IObservable<TSource> source,
			TSource defaultValue)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = Observable.CreateWithDisposable<TSource>(
				observer =>
				{
					bool hasValue = false;

					return source.Subscribe(
						value =>
						{
							hasValue = true;
							observer.OnNext(value);
						},
						observer.OnError,
						() =>
						{
							if (!hasValue)
								observer.OnNext(defaultValue);

							observer.OnCompleted();
						});
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
