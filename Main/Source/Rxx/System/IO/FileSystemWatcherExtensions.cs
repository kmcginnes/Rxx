using System.Collections.Generic;
using System.ComponentModel;
using System.Concurrency;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.IO
{
	/// <summary>
	/// Provides extension methods for <see cref="FileSystemWatcher"/>.
	/// </summary>
	public static class FileSystemWatcherExtensions
	{
		// http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.internalbuffersize.aspx
		private const int minBufferSize = 4096;
		private const int maxBufferSize = 65536;

		/// <summary>
		/// Creates an observable sequence of file system change notifications of the specified types.
		/// </summary>
		/// <param name="watcher">Watches the file system for changes.</param>
		/// <param name="changes">Specifies the types of changes to watch.</param>
		/// <returns>An observable sequence of file system change notifications.</returns>
		public static IObservable<FileSystemNotification> Watch(
			this FileSystemWatcher watcher,
			WatcherChangeTypes changes)
		{
			Contract.Requires(watcher != null);
			Contract.Ensures(Contract.Result<IObservable<FileSystemNotification>>() != null);

			var observable = Observable.Defer<FileSystemNotification>(() =>
				{
					var allEvents = CreateObservableEvents(watcher, changes);

					var error = Observable.FromEvent<ErrorEventHandler, ErrorEventArgs>(
						eh => eh.Invoke,
						eh => watcher.Error += eh,
						eh => watcher.Error -= eh);

					allEvents = allEvents.Merge(
						error.SelectMany(
							e => Observable.Throw<FileSystemNotification>(new FileSystemWatcherBufferOverflowException(e.EventArgs.GetException()))));

					watcher.EnableRaisingEvents = true;

					bool firstTime = true;

					return Observable.CreateWithDisposable<FileSystemNotification>(
						observer =>
						{
							return Scheduler.CurrentThread.Schedule(
								self =>
								{
									if (!firstTime)
									{
										int size = watcher.InternalBufferSize;
										int newSize = Math.Min(size + minBufferSize, maxBufferSize);

										Trace.TraceWarning(Rxx.Properties.Text.FileSystemWatcherBufferChangeFormat, size, newSize);

										watcher.InternalBufferSize = newSize;
									}

									firstTime = false;

									allEvents.Subscribe(
										observer.OnNext,
										ex =>
										{
											if (ex is FileSystemWatcherBufferOverflowException)
											{
												if (watcher.InternalBufferSize < maxBufferSize)
												{
													Trace.TraceWarning(Rxx.Properties.Text.FileSystemWatcherBufferOverflowFormat, ex.Message);

													self();
												}
												else
												{
													Trace.TraceError(Rxx.Properties.Text.FileSystemWatcherBufferOverflowFormat, ex.Message);
												}
											}
											else
											{
												observer.OnError(ex);
											}
										},
										observer.OnCompleted);
								});
						});
				});

			Contract.Assume(observable != null);

			return observable;
		}

		private static IObservable<FileSystemNotification> CreateObservableEvents(
			FileSystemWatcher watcher,
			WatcherChangeTypes changes)
		{
			Contract.Requires(watcher != null);
			Contract.Ensures(Contract.Result<IObservable<FileSystemNotification>>() != null);

			IObservable<FileSystemNotification> allEvents = null;

			var events = CreateObservableEventsWithoutRenamed(watcher, changes);

			if (events != null)
			{
				allEvents = from e in events
										select new FileSystemNotification(
											e.EventArgs.ChangeType,
											e.EventArgs.Name,
											e.EventArgs.FullPath);
			}

			if (changes.HasFlag(WatcherChangeTypes.Renamed))
			{
				var renamed = Observable
					.FromEvent<RenamedEventHandler, RenamedEventArgs>(
						eh => eh.Invoke,
						eh => watcher.Renamed += eh,
						eh => watcher.Renamed -= eh)
					.Select(e => new FileSystemNotification(
						e.EventArgs.OldName,
						e.EventArgs.OldFullPath,
						e.EventArgs.Name,
						e.EventArgs.FullPath));

				allEvents = allEvents == null ? renamed : allEvents.Merge(renamed);
			}

			if (allEvents == null)
				throw new InvalidEnumArgumentException("changes", (int) changes, typeof(WatcherChangeTypes));

			return allEvents;
		}

		private static IObservable<IEvent<FileSystemEventArgs>> CreateObservableEventsWithoutRenamed(
			FileSystemWatcher watcher,
			WatcherChangeTypes changes)
		{
			Contract.Requires(watcher != null);

			IObservable<IEvent<FileSystemEventArgs>> events = null;

			if (changes.HasFlag(WatcherChangeTypes.Changed))
			{
				events = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
					eh => eh.Invoke,
					eh => watcher.Changed += eh,
					eh => watcher.Changed -= eh);
			}

			if (changes.HasFlag(WatcherChangeTypes.Created))
			{
				var created = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
					eh => eh.Invoke,
					eh => watcher.Created += eh,
					eh => watcher.Created -= eh);

				events = events == null ? created : events.Merge(created);
			}

			if (changes.HasFlag(WatcherChangeTypes.Deleted))
			{
				var deleted = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
					eh => eh.Invoke,
					eh => watcher.Deleted += eh,
					eh => watcher.Deleted -= eh);

				events = events == null ? deleted : events.Merge(deleted);
			}

			return events;
		}
	}
}