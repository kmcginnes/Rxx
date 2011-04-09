using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Rxx.Labs
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(observerFactory != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

			Contract.Assume(source != null);

			var result = source.Do(new TypeCoercingObserver<T, object>(observerFactory()));

			Contract.Assume(result != null);

			return result;
		}

		public static void Run<T>(this IEnumerable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(source != null);
			Contract.Requires(observerFactory != null);

			source.Run(new TypeCoercingObserver<T, object>(observerFactory()));
		}
	}
}
