using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Rxx.Labs
{
	internal static class ObservableExtensions
	{
		public static IObservable<T> Do<T>(this IObservable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(observerFactory != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			Contract.Assume(source != null);

			var result = source.Do(new TypeCoercingObserver<T, object>(observerFactory()));

			Contract.Assume(result != null);

			return result;
		}

		public static void Run<T>(this IObservable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(observerFactory != null);

			Contract.Assume(source != null);

			source.Run(new TypeCoercingObserver<T, object>(observerFactory()));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Func<IObserver<object>> observerFactory)
		{
			Contract.Requires(observerFactory != null);
			Contract.Ensures(Contract.Result<IDisposable>() != null);

			Contract.Assume(source != null);

			var observer = new TypeCoercingObserver<T, object>(observerFactory());

			return source.Subscribe(observer);
		}
	}
}
