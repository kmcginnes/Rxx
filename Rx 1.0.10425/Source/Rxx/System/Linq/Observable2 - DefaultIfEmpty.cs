using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <summary>
		/// Returns the elements of the specified sequence or the type parameter's default value 
		/// in a singleton observable if the sequence is empty.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable to be defaulted if empty.</param>
		/// <returns>The specified observable sequence if it's not empty; otherwise, the default value 
		/// of <typeparamref name="TSource"/> in a singleton observable.</returns>
		public static IObservable<TSource> DefaultIfEmpty<TSource>(
			this IObservable<TSource> source)
		{
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return DefaultIfEmpty(source, default(TSource));
		}

		/// <summary>
		/// Returns the elements of the specified sequence or the specified <paramref name="defaultValue"/>
		/// in a singleton observable if the sequence is empty.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable to be defaulted if empty.</param>
		/// <param name="defaultValue">The default value if the specified observable sequence is empty.</param>
		/// <returns>The specified observable sequence if it's not empty; otherwise, specified <paramref name="defaultValue"/> 
		/// in a singleton observable.</returns>
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
