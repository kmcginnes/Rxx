using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Rxx;
using Rxx.Properties;

namespace System.ComponentModel
{
	public static class PropertyDescriptorExtensions
	{
		public static IObservable<IEvent<PropertyChangedEventArgs>> PropertyChanged(
			this PropertyDescriptor property,
			object source)
		{
			Contract.Requires(property != null);
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<IEvent<PropertyChangedEventArgs>>>() != null);

			if (!property.SupportsChangeEvents)
				throw new ArgumentException(Errors.PropertyDoesNotSupportChangeEvents, "property");

			var observable =
				from e in Observable.FromEvent<EventHandler, EventArgs>(
					handler => handler.Invoke,
					handler => property.AddValueChanged(source, handler),
					handler => property.RemoveValueChanged(source, handler))
				select Event.Create(
					e.Sender,
					e.EventArgs as PropertyChangedEventArgs ?? new PropertyChangedEventArgs(property.Name));

			Contract.Assume(observable != null);

			return observable;
		}

		public static IObservable<IEvent<EventArgs>> EventRaised(
			this EventDescriptor @event,
			object source)
		{
			Contract.Requires(@event != null);
			Contract.Requires(source != null);
			Contract.Ensures(Contract.Result<IObservable<IEvent<EventArgs>>>() != null);

			var observable = @event.EventType == typeof(EventHandler)
				? Observable.FromEvent<EventHandler, EventArgs>(
						handler => handler.Invoke,
						handler => @event.AddEventHandler(source, handler),
						handler => @event.RemoveEventHandler(source, handler))
				: new EventProxyObservable(source, @event);

			Contract.Assume(observable != null);

			return observable;
		}
	}
}
