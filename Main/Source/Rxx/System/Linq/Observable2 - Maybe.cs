using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <summary>
		/// Returns the elements of the specified sequence as a sequence of <see cref="Maybe{T}"/>, 
		/// starting with <see cref="System.Maybe{T}.Empty"/>.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="source">The observable to be projected into <see cref="Maybe{T}"/> values.</param>
		/// <returns>A sequence of <see cref="Maybe{T}"/> values that contain the values from the specified
		/// observable, starting with <see cref="System.Maybe{T}.Empty"/>.</returns>
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