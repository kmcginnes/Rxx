using System.Diagnostics.Contracts;

namespace System.Linq
{
	public static partial class PairedObservable
	{
		/// <remarks>
		/// <see cref="Combine"/> is similar to <see cref="Observable.CombineLatest"/>.
		/// </remarks>
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
