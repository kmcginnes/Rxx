﻿using System.Concurrency;
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

		/// <summary>
		/// Concurrently invokes the specified factory to create observables as fast and often as possible and subscribes to all of them
		/// up to the default maximum concurrency.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="sourceFactory">A function that returns observables.</param>
		/// <remarks>
		/// <para>
		/// <see cref="Serve{TSource}(Func{IObservable{TSource}})"/> is similar to <see cref="Observable.Merge{TSource}(IObservable{IObservable{TSource}},int)"/>
		/// except that it does not obey the serializability guarantee for <see cref="IObserver{T}.OnNext"/> that is recommended in the 
		/// <strong>Rx Design Guidelines</strong>.  Its behavior is therefore more suitable for hosting environments that must service multiple
		/// requests concurrently without blocking during observations.  This comes with the loss of automatic synchronization and thread-safety 
		/// that is normally provided by Rx operators, thus consumers are responsible for ensuring the thread-safety of all observers themselves.
		/// </para>
		/// </remarks>
		/// <returns>A concurrent observable sequence.</returns>
		public static IObservable<TSource> Serve<TSource>(this Func<IObservable<TSource>> sourceFactory)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return Serve(sourceFactory, ex => false);
		}

		/// <summary>
		/// Concurrently invokes the specified factory to create observables as fast and often as possible and subscribes to all of them
		/// up to the default maximum concurrency.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="sourceFactory">A function that returns observables.</param>
		/// <param name="onError">Determines whether an error should fault the entire observable sequence.</param>
		/// <remarks>
		/// <para>
		/// <see cref="Serve{TSource}(Func{IObservable{TSource}},Func{Exception,bool})"/> is similar to <see cref="Observable.Merge{TSource}(IObservable{IObservable{TSource}},int)"/>
		/// except that it does not obey the serializability guarantee for <see cref="IObserver{T}.OnNext"/> that is recommended in the 
		/// <strong>Rx Design Guidelines</strong>.  Its behavior is therefore more suitable for hosting environments that must service multiple
		/// requests concurrently without blocking during observations.  This comes with the loss of automatic synchronization and thread-safety 
		/// that is normally provided by Rx operators, thus consumers are responsible for ensuring the thread-safety of all observers themselves.
		/// </para>
		/// <para>
		/// Furthermore, <see cref="Serve{TSource}(Func{IObservable{TSource}},Func{Exception,bool})"/> provides an <paramref name="onError"/> parameter that 
		/// allows the caller to decide whether an <see cref="Exception"/> is fatal and should fault the entire sequence.  This function 
		/// should return <see langword="true"/> to indicate that an <see cref="Exception"/> has been handled, thus preventing the sequence
		/// from being faulted; otherwise, return <see langword="false"/> to fault the sequence and halt processing as soon as possible.
		/// In the latter case, the <see cref="Exception"/> is then passed to the observer of the sequence as is the normal behavior in Rx.
		/// </para>
		/// </remarks>
		/// <returns>A concurrent observable sequence.</returns>
		public static IObservable<TSource> Serve<TSource>(this Func<IObservable<TSource>> sourceFactory, Func<Exception, bool> onError)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			Contract.Assume(DefaultMaxConcurrent > 0);

			return Serve(sourceFactory, DefaultMaxConcurrent, onError);
		}

		/// <summary>
		/// Concurrently invokes the specified factory to create observables as fast and often as possible and subscribes to all of them
		/// up to the specified maximum concurrency.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="sourceFactory">A function that returns observables.</param>
		/// <param name="maxConcurrent">The maximum number of observables to be subscribed simultaneously.</param>
		/// <remarks>
		/// <para>
		/// <see cref="Serve{TSource}(Func{IObservable{TSource}},int)"/> is similar to <see cref="Observable.Merge{TSource}(IObservable{IObservable{TSource}},int)"/>
		/// except that it does not obey the serializability guarantee for <see cref="IObserver{T}.OnNext"/> that is recommended in the 
		/// <strong>Rx Design Guidelines</strong>.  Its behavior is therefore more suitable for hosting environments that must service multiple
		/// requests concurrently without blocking during observations.  This comes with the loss of automatic synchronization and thread-safety 
		/// that is normally provided by Rx operators, thus consumers are responsible for ensuring the thread-safety of all observers themselves.
		/// </para>
		/// </remarks>
		/// <returns>A concurrent observable sequence.</returns>
		public static IObservable<TSource> Serve<TSource>(this Func<IObservable<TSource>> sourceFactory, int maxConcurrent)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(maxConcurrent > 0);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			return Serve(sourceFactory, maxConcurrent, ex => false);
		}

		/// <summary>
		/// Concurrently invokes the specified factory to create observables as fast and often as possible and subscribes to all of them
		/// up to the specified maximum concurrency.
		/// </summary>
		/// <typeparam name="TSource">The object that provides notification information.</typeparam>
		/// <param name="sourceFactory">A function that returns observables.</param>
		/// <param name="maxConcurrent">The maximum number of observables to be subscribed simultaneously.</param>
		/// <param name="onError">Determines whether an error should fault the entire observable sequence.</param>
		/// <remarks>
		/// <para>
		/// <see cref="Serve{TSource}(Func{IObservable{TSource}},int,Func{Exception,bool})"/> is similar to <see cref="Observable.Merge{TSource}(IObservable{IObservable{TSource}},int)"/>
		/// except that it does not obey the serializability guarantee for <see cref="IObserver{T}.OnNext"/> that is recommended in the 
		/// <strong>Rx Design Guidelines</strong>.  Its behavior is therefore more suitable for hosting environments that must service multiple
		/// requests concurrently without blocking during observations.  This comes with the loss of automatic synchronization and thread-safety 
		/// that is normally provided by Rx operators, thus consumers are responsible for ensuring the thread-safety of all observers themselves.
		/// </para>
		/// <para>
		/// Furthermore, <see cref="Serve{TSource}(Func{IObservable{TSource}},int,Func{Exception,bool})"/> provides an <paramref name="onError"/> parameter that 
		/// allows the caller to decide whether an <see cref="Exception"/> is fatal and should fault the entire sequence.  This function 
		/// should return <see langword="true"/> to indicate that an <see cref="Exception"/> has been handled, thus preventing the sequence
		/// from being faulted; otherwise, return <see langword="false"/> to fault the sequence and halt processing as soon as possible.
		/// In the latter case, the <see cref="Exception"/> is then passed to the observer of the sequence as is the normal behavior in Rx.
		/// </para>
		/// </remarks>
		/// <returns>A concurrent observable sequence.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "The CompositeDisposable is disposed by the underlying observable.")]
		public static IObservable<TSource> Serve<TSource>(this Func<IObservable<TSource>> sourceFactory, int maxConcurrent, Func<Exception, bool> onError)
		{
			Contract.Requires(sourceFactory != null);
			Contract.Requires(maxConcurrent > 0);
			Contract.Requires(onError != null);
			Contract.Ensures(Contract.Result<IObservable<TSource>>() != null);

			var observable = Observable.CreateWithDisposable<TSource>(
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
