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
	/// <summary>
	/// Wraps an object with a dynamic wrapper that converts normal properties, methods and events into observable sequences.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Methods are converted to asynchronous invocations much like <see cref="Observable.Start(Action)"/> and <see cref="System.Linq.Observable.Start{TResult}(Func{TResult})"/>.
	/// </para>
	/// <para>
	/// Properties are converted to observable sequences of property changed notifications.
	/// </para>
	/// <para>
	/// Events are converted to observable sequences of <see cref="System.Collections.Generic.IEvent{TEventArgs}"/>, with strong-typed <see cref="EventArgs"/>.
	/// </para>
	/// </remarks>
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

		/// <summary>
		/// Wraps the specified object with a <see langword="dynamic"/> wrapper implemented by <see cref="ObservableDynamicObject"/>.
		/// </summary>
		/// <param name="source">The object to be wrapped.</param>
		/// <returns>The specified <paramref name="source"/> as a <see langword="dynamic"/> object.</returns>
		public static dynamic Create(object source)
		{
			Contract.Requires(source != null);

			return new ObservableDynamicObject(source);
		}

		/// <summary>
		/// Gets the names of the dynamic members.
		/// </summary>
		/// <returns>Sequence of dynamic member names.</returns>
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

			return ComponentReflection.GetMembers(source).Select(member => member.Name);
		}

		/// <summary>
		/// Tries to invoke the specified member.
		/// </summary>
		/// <param name="binder">The binder.</param>
		/// <param name="args">The member's arguments.</param>
		/// <param name="result">The result, if any.</param>
		/// <returns><see langword="true"/> if the member is invoked; otherwise, <see langword="false"/>.</returns>
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

		/// <summary>
		/// Tries to set the specified member to the specified <paramref name="value"/>.
		/// </summary>
		/// <param name="binder">The binder.</param>
		/// <param name="value">The value to be set.</param>
		/// <returns><see langword="true"/> if the member is set; otherwise, <see langword="false"/>.</returns>
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

		/// <summary>
		/// Tries to get a value from the specified member.
		/// </summary>
		/// <param name="binder">The binder.</param>
		/// <param name="result">The value that was retrieved from the member.</param>
		/// <returns><see langword="true"/> if the <paramref name="result"/> is retrieved; otherwise, <see langword="false"/>.</returns>
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