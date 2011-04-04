using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Rxx.Labs
{
	internal static class EnumerableExtensions
	{
		public static void Run<T>(this IEnumerable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(source != null);
			Contract.Requires(observerFactory != null);

			source.Run(new TypeCoercingObserver<T, object>(observerFactory()));
		}
	}
}
