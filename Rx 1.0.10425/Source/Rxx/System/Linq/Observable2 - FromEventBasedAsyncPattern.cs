using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Disposables;
using System.Threading;

namespace System.Linq
{
	public static partial class Observable2
	{
		/// <seealso href="http://msdn.microsoft.com/en-us/library/wewwczdw.aspx">
		/// Event-based Asynchronous Pattern Overview
		/// </seealso>
		public static IObservable<IEvent<TEventArgs>> FromEventBasedAsyncPattern<TDelegate, TEventArgs>(
			Func<EventHandler<TEventArgs>, TDelegate> conversion,
			Action<TDelegate> addHandler,
			Action<TDelegate> removeHandler,
			Action<object> start,
			Action cancel)
			where TEventArgs : AsyncCompletedEventArgs
		{
			Contract.Requires(conversion != null);
			Contract.Requires(addHandler != null);
			Contract.Requires(removeHandler != null);
			Contract.Requires(start != null);
			Contract.Requires(cancel != null);
			Contract.Ensures(Contract.Result<IObservable<IEvent<TEventArgs>>>() != null);

			return FromEventBasedAsyncPattern(
				conversion,
				addHandler,
				removeHandler,
				start,
				cancel,
				() => { });
		}

		/// <seealso href="http://msdn.microsoft.com/en-us/library/wewwczdw.aspx">
		/// Event-based Asynchronous Pattern Overview
		/// </seealso>
		public static IPairedObservable<IEvent<TProgressEventArgs>, IEvent<TEventArgs>> FromEventBasedAsyncPattern<TDelegate, TEventArgs, TProgressDelegate, TProgressEventArgs>(
			Func<EventHandler<TEventArgs>, TDelegate> conversion,
			Action<TDelegate> addHandler,
			Action<TDelegate> removeHandler,
			Func<EventHandler<TProgressEventArgs>, TProgressDelegate> progressConversion,
			Action<TProgressDelegate> addProgressHandler,
			Action<TProgressDelegate> removeProgressHandler,
			Action<object> start,
			Action cancel)
			where TEventArgs : AsyncCompletedEventArgs
			where TProgressEventArgs : ProgressChangedEventArgs
		{
			Contract.Requires(conversion != null);
			Contract.Requires(addHandler != null);
			Contract.Requires(removeHandler != null);
			Contract.Requires(progressConversion != null);
			Contract.Requires(addProgressHandler != null);
			Contract.Requires(removeProgressHandler != null);
			Contract.Requires(start != null);
			Contract.Requires(cancel != null);
			Contract.Ensures(Contract.Result<IPairedObservable<IEvent<TProgressEventArgs>, IEvent<TEventArgs>>>() != null);

			TProgressDelegate progressHandler = default(TProgressDelegate);
			int isProgressHandlerRemoved = 0;

			Action tryRemoveProgressHandler = () =>
			{
				if (Interlocked.Exchange(ref isProgressHandlerRemoved, 1) == 0)
				{
					removeProgressHandler(progressHandler);
				}
			};

			var progressSubject = new Subject<IEvent<TProgressEventArgs>>();

			var response =
				FromEventBasedAsyncPattern<TDelegate, TEventArgs>(
					conversion,
					addHandler,
					removeHandler,
					token =>
					{
						progressHandler = progressConversion((sender, e) =>
						{
							if (object.ReferenceEquals(e.UserState, token))
							{
								progressSubject.OnNext(Event.Create(sender, e));
							}
						});

						addProgressHandler(progressHandler);

						start(token);
					},
					cancel,
					tryRemoveProgressHandler);

			var observable = progressSubject.TakeUntil(response.DefaultIfEmpty());

			Contract.Assume(observable != null);

			return observable.Pair(response);
		}

		/// <seealso href="http://msdn.microsoft.com/en-us/library/wewwczdw.aspx">
		/// Event-based Asynchronous Pattern Overview
		/// </seealso>
		[SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines",
			Justification = "False positive.  Reported bug in CodePlex as user 'davedev'.")]
		private static IObservable<IEvent<TEventArgs>> FromEventBasedAsyncPattern<TDelegate, TEventArgs>(
			Func<EventHandler<TEventArgs>, TDelegate> conversion,
			Action<TDelegate> addHandler,
			Action<TDelegate> removeHandler,
			Action<object> start,
			Action cancel,
			Action canceledOrCompleted)
			where TEventArgs : AsyncCompletedEventArgs
		{
			Contract.Requires(conversion != null);
			Contract.Requires(addHandler != null);
			Contract.Requires(removeHandler != null);
			Contract.Requires(start != null);
			Contract.Requires(cancel != null);
			Contract.Requires(canceledOrCompleted != null);
			Contract.Ensures(Contract.Result<IObservable<IEvent<TEventArgs>>>() != null);

			var subject = new AsyncSubject<IEvent<TEventArgs>>();
			TDelegate handler = default(TDelegate);

			object token = new object();
			bool completed = false;
			int isHandlerRemoved = 0, wasCanceled = 0;

			Action tryRemoveHandler = () =>
			{
				if (Interlocked.Exchange(ref isHandlerRemoved, 1) == 0)
				{
					removeHandler(handler);
					canceledOrCompleted();
				}
			};

			handler = conversion((sender, e) =>
			{
				if (object.ReferenceEquals(e.UserState, token))
				{
					completed = true;

					// ensure the handler is removed in case there are no subscribers
					tryRemoveHandler();

					if (e.Error != null)
					{
						subject.OnError(e.Error);
					}
					else
					{
						if (!e.Cancelled)
						{
							subject.OnNext(Event.Create(sender, e));
						}

						subject.OnCompleted();
					}
				}
			});

			addHandler(handler);

			start(token);

			var observable = Observable.CreateWithDisposable<IEvent<TEventArgs>>(
				observer => new CompositeDisposable(
					subject.Subscribe(observer),
					Disposable.Create(() =>
					{
						if (!completed)
						{
							if (Interlocked.Exchange(ref wasCanceled, 1) == 0)
							{
								cancel();
							}

							tryRemoveHandler();
						}
					})));

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
