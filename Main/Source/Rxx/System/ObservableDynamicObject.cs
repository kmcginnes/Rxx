using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Rxx;
using Rxx.Properties;

namespace System
{
	public sealed partial class ObservableDynamicObject : DynamicObject
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		private readonly object source;
		#endregion

		#region Constructors
		private ObservableDynamicObject(object source)
		{
			Contract.Requires(source != null);

			this.source = source;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(source != null);
		}

		public static dynamic Create(object source)
		{
			Contract.Requires(source != null);

			return new ObservableDynamicObject(source);
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

			return ComponentReflection.GetMembers(source).Select(member => member.Name);
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

			Contract.Assume(binder != null);
			Contract.Assume(args != null);

			MethodInfo method = source.GetType().GetMethod(
				binder.Name,
				BindingFlags.Public | BindingFlags.Instance,
				Type.DefaultBinder,
				args.Select(a => a.GetType()).ToArray(),
				null);

			if (method == null)
			{
				result = null;
				return false;
			}

			Func<object, object[], object> invoke = method.Invoke;

			var invokeAsync = invoke.ToAsync();

			Contract.Assume(invokeAsync != null);

			IObservable<object> observable = invokeAsync(source, args);

			Contract.Assume(observable != null);

			if (method.ReturnType == typeof(void))
			{
				result = observable.Coerce(typeof(Unit));
			}
			else
			{
				result = observable.Coerce(method.ReturnType);
			}

			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			Contract.Assume(binder != null);

			var comparison = binder.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

			Contract.Assume(binder.Name != null);

			var property = ComponentReflection.GetProperty(source, binder.Name, comparison);

			if (property == null)
				return false;

			if (property.IsReadOnly)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Errors.PropertyIsReadOnly, property.Name));
			}

			Action<object, object> action = property.SetValue;

			var invokeAsync = action.ToAsync();

			Contract.Assume(invokeAsync != null);

			invokeAsync(source, value);

			return true;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

			Contract.Assume(binder != null);
			Contract.Assume(binder.Name != null);

			var comparison = binder.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

			return TryGetPropertyObservable(binder.Name, comparison, out result)
					|| TryGetEventObservable(binder.Name, comparison, out result);
		}

		private bool TryGetPropertyObservable(string propertyName, StringComparison comparison, out object result)
		{
			Contract.Requires(propertyName != null);
			Contract.Requires(Enum.IsDefined(typeof(StringComparison), comparison));
			Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

			var property = ComponentReflection.GetProperty(source, propertyName, comparison);

			if (property == null)
			{
				result = null;
				return false;
			}

			var changed = property.PropertyChanged(source).Select(_ => property.GetValue(source));

			Contract.Assume(changed != null);
			Contract.Assume(property.PropertyType != null);

			result = changed.Coerce(property.PropertyType);

			return true;
		}

		private bool TryGetEventObservable(string eventName, StringComparison comparison, out object result)
		{
			Contract.Requires(eventName != null);
			Contract.Requires(Enum.IsDefined(typeof(StringComparison), comparison));
			Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

			var @event = ComponentReflection.GetEvent(source, eventName, comparison);

			if (@event == null)
			{
				result = null;
				return false;
			}

			Type eventArgsType;

			if (@event.EventType == typeof(EventHandler))
			{
				eventArgsType = typeof(EventArgs);
			}
			else if (!@event.EventType.IsGenericType || @event.EventType.GetGenericTypeDefinition() != typeof(EventHandler<>))
			{
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, Errors.EventIsNotCompatibleWithEventHandler, eventName),
					"eventName");
			}
			else
			{
				eventArgsType = @event.EventType.GetGenericArguments()[0];

				Contract.Assume(eventArgsType != null);

				if (!typeof(EventArgs).IsAssignableFrom(eventArgsType))
				{
					throw new ArgumentException(
						string.Format(CultureInfo.CurrentCulture, Errors.EventIsNotCompatibleWithEventArgs, eventName),
						"eventName");
				}
			}

			result = @event.EventRaised(source).Coerce(eventArgsType);

			return true;
		}
		#endregion
	}
}