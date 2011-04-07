using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Interactive
{
	[TestClass]
	public partial class TraceTests : RxxTraceTests
	{
		[TestMethod]
		public void IxTestTrace()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.Trace().Run();

			AssertEqual(Listener.Messages, Concat(
				Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)),
				TraceDefaults.DefaultOnCompleted()));

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnNext()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext().Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)));

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnNextFormat()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext("OnNext: {0}").Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnNextLazyMessage()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext(value => "OnNext: " + value).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnError()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError().Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(ex));

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnErrorFormat()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError("OnError: {0}").Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, "OnError: " + ex.ToString());

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnErrorLazyMessage()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError(error => "OnError: " + error.Message).Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, "OnError: " + ex.Message);

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnCompleted()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted().Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted());

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnCompletedFormat()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted("OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceOnCompletedLazyMessage()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted(() => "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");

			RemoveTraceListener();
		}
	}
}