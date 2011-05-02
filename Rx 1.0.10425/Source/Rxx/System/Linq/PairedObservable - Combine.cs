using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		/// <summary>
		/// Combines the latest values from both notification channels and projects the results into a new sequence.
		/// </summary>
		/// <typeparam name="TLeft">Type of the left notification channel.</typeparam>
		/// <typeparam name="TRight">Type of the right notification channel.</typeparam>
		/// <typeparam name="TResult">Type of the result.</typeparam>
		/// <param name="source">The observable from which values are combined.</param>
		/// <param name="selector">Combines values from both notification channels.</param>
		/// <remarks>
		/// <see cref="Combine"/> is similar to <see cref="Observable.CombineLatest"/>.
		/// </remarks>
		/// <returns>An observable of results from the combination of the latest values in both notification channels.</returns>
		public static IObservable<TResult> Combine<TLeft, TRight, TResult>(
			this IObservable<Either<TLeft, TRight>> source,
			Func<TLeft, TRight, TResult> selector)
		{
			Contract.Requires(source != null);
			Contract.Requires(selector != null);
			Contract.Ensures(Contract.Result<IObservable<Tuple<TLeft, TRight>>>() != null);

			var observable = source.Scan(
				Tuple.Create(Maybe<TLeft>.Empty, Maybe<TRight>.Empty),
				(acc, cur) =>
				{
					if (cur.IsLeft)
						return Tuple.Create(new Maybe<TLeft>(cur.Left), acc.Item2);
					else
						return Tuple.Create(acc.Item1, new Maybe<TRight>(cur.Right));
				})
				.Where(tuple => tuple.Item1.HasValue && tuple.Item2.HasValue)
				.Select(tuple => selector(tuple.Item1.Value, tuple.Item2.Value));

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
