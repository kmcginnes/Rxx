using System.Collections.Generic;
using System.Diagnostics;

namespace Rxx.UnitTests
{
	public abstract class RxxTraceTests : RxxTests
	{
		#region Public Properties
		public TestTraceListener Listener
		{
			get
			{
				return testListener;
			}
		}
		#endregion

		#region Private / Protected
		protected static int GetCurrentId()
		{
			return (int) typeof(IdentifiedTraceObserver<int>)
				.GetField("counter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
				.GetValue(null);
		}

		private readonly TestTraceListener testListener = new TestTraceListener();
		#endregion

		#region Constructors
		protected RxxTraceTests()
		{
		}
		#endregion

		#region Methods
		protected void AddTraceListener()
		{
			testListener.Clear();

			Trace.Listeners.Add(testListener);
		}

		protected void RemoveTraceListener()
		{
			Trace.Listeners.Remove(testListener);
		}

		protected TestTraceSource CreateTraceSource()
		{
			testListener.Clear();

			var source = new TestTraceSource();

			source.Listeners.Add(testListener);

			return source;
		}
		#endregion
	}
}
