using System;
using System.Collections.Generic;
using System.Concurrency;
using System.Linq;
using System.Reactive.Testing;

namespace Rxx.UnitTests
{
	public abstract class RxxTests
	{
		#region Public Properties
		#endregion

		#region Private / Protected
		#endregion

		#region Constructors
		protected RxxTests()
		{
		}
		#endregion

		#region Methods
		protected IEnumerable<object> Concat<T>(IEnumerable<T> first, params object[] second)
		{
			return first.Cast<object>().Concat(second);
		}

		protected IEnumerable<string> Concat<T>(IEnumerable<string> first, params object[] second)
		{
			return first.Concat(second.Select(value => value.ToString()));
		}

		protected void AssertEqual<T>(IEnumerable<T> first, params T[] second)
		{
			first.AssertEqual(second);
		}

		protected void AssertEqual<T>(IEnumerable<T> first, params IEnumerable<T>[] second)
		{
			first.AssertEqual(second.Concat());
		}

		protected void AssertEqual<T>(IEnumerable<string> first, params T[] second)
		{
			first.AssertEqual(second.Select(value => value.ToString()));
		}

		protected void AssertEqual<T>(IEnumerable<string> first, params IEnumerable<T>[] second)
		{
			first.AssertEqual(second.Concat().Select(value => value.ToString()));
		}

		protected void AssertEqual<T>(IObservable<T> first, params T[] second)
		{
			first.AssertEqual(second.ToObservable(Scheduler.Immediate));
		}

		protected void AssertEqual<T>(IObservable<T> first, params IObservable<T>[] second)
		{
			first.AssertEqual(second.Concat());
		}

		protected void AssertEqual<T>(IObservable<string> first, params T[] second)
		{
			first.AssertEqual(second.Select(value => value.ToString()).ToObservable(Scheduler.Immediate));
		}

		protected void AssertEqual<T>(IObservable<string> first, params IObservable<T>[] second)
		{
			first.AssertEqual(second.Concat().Select(value => value.ToString()));
		}
		#endregion
	}
}
