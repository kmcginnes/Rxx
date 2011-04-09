using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Collections.Generic
{
	internal sealed class EventProxyObservable : IObservable<IEvent<EventArgs>>
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly IObservable<IEvent<EventArgs>> observable;
		#endregion

		#region Constructors
		public EventProxyObservable(object source, EventDescriptor @event)
		{
			Contract.Requires(source != null);
			Contract.Requires(@event != null);

			Delegate onEvent = Delegate.CreateDelegate(
				@event.EventType,
				this,
				GetType().GetMethod("OnEvent"));

			observable = Observable.FromEvent<EventHandler, EventArgs>(
				handler => handler.Invoke,
				handler =>
				{
					Proxy += handler;
					@event.AddEventHandler(source, onEvent);
				},
				handler =>
				{
					Proxy -= handler;
					@event.RemoveEventHandler(source, onEvent);
				});
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(observable != null);
		}

		public IDisposable Subscribe(IObserver<IEvent<EventArgs>> observer)
		{
			return observable.Subscribe(observer);
		}

		public void OnEvent(object sender, EventArgs e)
		{
			var proxy = Proxy;

			if (proxy != null)
				proxy(sender, e);
		}
		#endregion

		#region Events
		private event EventHandler Proxy;
		#endregion
	}
}
