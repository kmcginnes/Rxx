using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using Rxx.Properties;

namespace System.Linq.Expressions
{
	public static class ExpressionExtensions
	{
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
			Justification = "Full type may be required for C# compiler support; regardless, this may be clearer to callers.")]
		public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
		{
			Contract.Requires(property != null);
			Contract.Ensures(Contract.Result<PropertyInfo>() != null);

			var body = property.Body as MemberExpression;

			if (body == null)
				goto NotAProperty;

			var propertyInfo = body.Member as PropertyInfo;

			if (propertyInfo == null)
				goto NotAProperty;

			return propertyInfo;

		NotAProperty:
			throw new ArgumentException(Errors.PropertyExpressionNotUnderstood, "property");
		}

		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
			Justification = "This is the simplest API possible in this scenario.")]
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
			Justification = "Full type may be required for C# compiler support; regardless, this may be clearer to callers.")]
		[SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate",
			Justification = "Generics would require an explicit cast internally.  By typing owner as object callers are responsible.")]
		public static PropertyInfo GetPropertyInfo<TValue>(this Expression<Func<TValue>> property, out object owner)
		{
			Contract.Requires(property != null);
			Contract.Ensures(Contract.Result<PropertyInfo>() != null);
			Contract.Ensures(Contract.ValueAtReturn(out owner) != null);

			var body = property.Body as MemberExpression;

			if (body == null)
				goto NotAProperty;

			var propertyInfo = body.Member as PropertyInfo;

			if (propertyInfo == null)
				goto NotAProperty;

			var parent = body.Expression as MemberExpression;

			if (parent == null)
				goto NotUnderstood;

			owner = GetOwner(parent);

			if (!propertyInfo.DeclaringType.IsAssignableFrom(owner.GetType()))
				goto NotContained;

			return propertyInfo;

		NotAProperty:
			throw new ArgumentException(Errors.PropertyExpressionNotUnderstood, "property");

		NotUnderstood:
			throw new ArgumentException(Errors.PropertyExpressionOwnerNotUnderstood, "property");

		NotContained:
			throw new ArgumentException(Errors.PropertyExpresssionOwnerNotDetermined, "property");
		}

		private static object GetOwner(MemberExpression property)
		{
			Contract.Requires(property != null);
			Contract.Ensures(Contract.Result<object>() != null);

			object owner;

			var memberChain = new Stack<MemberInfo>();

			do
			{
				FieldInfo fieldInfo = null;
				PropertyInfo propertyInfo = property.Member as PropertyInfo;

				if (propertyInfo == null)
				{
					fieldInfo = property.Member as FieldInfo;

					if (fieldInfo == null)
						throw new ArgumentException(Errors.PropertyExpressionTooComplex, "property");
				}
				else
				{
					if (propertyInfo.GetIndexParameters().Length != 0)
						throw new ArgumentException(Errors.PropertyExpressionContainsIndexer, "property");
				}

				var parentReference = property.Expression as ConstantExpression;

				if (parentReference != null)
				{
					owner = fieldInfo == null
						? propertyInfo.GetValue(parentReference.Value, null)
						: fieldInfo.GetValue(parentReference.Value);

					if (owner == null)
						throw new ArgumentException(Errors.PropertyExpresssionOwnerNotDetermined, "property");
					break;
				}
				else
				{
					if (fieldInfo == null)
						memberChain.Push(propertyInfo);
					else
						memberChain.Push(fieldInfo);

					property = property.Expression as MemberExpression;

					if (property == null)
						throw new ArgumentException(Errors.PropertyExpressionTooComplex, "property");
				}
			}
			while (true);

			while (memberChain.Count > 0)
			{
				var member = memberChain.Pop();

				var fieldInfo = member as FieldInfo;

				if (fieldInfo == null)
				{
					var propertyInfo = (PropertyInfo) member;

					Contract.Assume(propertyInfo != null);

					owner = propertyInfo.GetValue(owner, null);
				}
				else
					owner = fieldInfo.GetValue(owner);

				if (owner == null)
					throw new ArgumentException(Errors.PropertyExpresssionOwnerNotDetermined, "property");
			}

			return owner;
		}
	}
}
