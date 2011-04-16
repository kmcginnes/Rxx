using System.Concurrency;
using System.Diagnostics.Contracts;
using System.Disposables;
using System.Threading;

namespace System.Linq
{
	public static partial class Observable2
	{
		internal static readonly int DefaultMaxConcurrent = GetDefaultMaxConcurrent();

		private static int GetDefaultMaxConcurrent()
		{
			Contract.Ensures(Contract.Result<int>() > 0);

			int worker, io;
			ThreadPool.GetMaxThreads(out worker, out io);

			// TODO: This is an arbitrary formula.  Do some research to find a better solution.
			int maxConcurrent = (io / 8) * Environment.ProcessorCount;

			Contract.Assume(maxConcurrent > 0);

			return maxConcurrent;
		}

		public static IObservable<T> Serve<T>(this Func<IObservable<T>> sourceFactory)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			return Serve(sourceFactory, ex => false);
		}

		public static IObservable<T> Serve<T>(this Func<IObservable<T>> sourceFactory, Func<Exception, bool> onError)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			Contract.Assume(DefaultMaxConcurrent > 0);

			return Serve(sourceFactory, DefaultMaxConcurrent, onError);
		}

		public static IObservable<T> Serve<T>(this Func<IObservable<T>> sourceFactory, int maxConcurrent)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			return Serve(sourceFactory, maxConcurrent, ex => false);
		}

		/// <remarks>
		/// <para>
		/// <see cref="Serve{T}(Func{TResult},int,Func{T,TResult})"/> is similar to <see cref="Observable.Merge{TSource}(IObservable{T},int)"/>
		/// except that it does not obey the serializability guarantee for <see cref="IObserver{T}.OnNext"/> that is recommended in the 
		/// <strong>Rx Design Guidelines</strong>.  Its behavior is therefore more suitable for hosting environments that must service multiple
		/// requests concurrently without blocking during observations.  This comes with the loss of automatic synchronization and thread-safety 
		/// that is normally provided by Rx operators, thus consumers are responsible for ensuring the thread-safety of all observers themselves.
		/// </para>
		/// <para>
		/// Furthermore, <see cref="Serve{T}(Func{TResult},int,Func{T,TResult})"/> provides an <paramref name="onError"/> parameter that 
		/// allows the caller to decide whether an <see cref="Exception"/> is fatal and should fault the entire sequence.  This function 
		/// should return <see langword="true"/> to indicate that an <see cref="Exception"/> has been handled, thus preventing the sequence
		/// from being faulted; otherwise, return <see langword="false"/> to fault the sequence and halt processing as soon as possible.
		/// In the latter case, the <see cref="Exception"/> is then passed to the observer of the sequence as is the normal Rx behavior.
		/// </para>
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "The CompositeDisposable is disposed by the underlying observable.")]
		public static IObservable<T> Serve<T>(this Func<IObservable<T>> sourceFactory, int maxConcurrent, Func<Exception, bool> onError)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(maxConcurrent > 0);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IObservable<T>>() != null);

			var observable = Observable.CreateWithDisposable<T>(
				observer =>
				{
					var disposables = new CompositeDisposable();

					for (int i = 0; i < maxConcurrent; i++)
					{
						var current = new MutableDisposable();

						disposables.Add(current);
						disposables.Add(
							Scheduler.CurrentThread.Schedule(self =>
								current.SetDisposableIndirectly(() =>
									sourceFactory().Subscribe(
										observer.OnNext,
										ex =>
										{
											if (!onError(ex))
												observer.OnError(ex);
										},
										self))));
					}

					return disposables;
				});

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
