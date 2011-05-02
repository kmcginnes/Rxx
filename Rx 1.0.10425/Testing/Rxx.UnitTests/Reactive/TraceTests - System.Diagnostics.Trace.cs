using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Reactive
{
	[TestClass]
	public partial class TraceTests : RxxTraceTests
	{
		[TestMethod]
		public void RxTestTrace()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.Trace().Run();

			AssertEqual(Listener.Messages, Concat(
				Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)),
				TraceDefaults.DefaultOnCompleted()));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnNext()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext().Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnNextFormat()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext("OnNext: {0}").Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnNextLazyMessage()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext(value => "OnNext: " + value).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnError()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError().Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(ex));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnErrorFormat()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError("OnError: {0}").Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, "OnError: " + ex.ToString());

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnErrorLazyMessage()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError(error => "OnError: " + error.Message).Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, "OnError: " + ex.Message);

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnCompleted()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted().Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted());

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnCompletedFormat()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted("OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceOnCompletedLazyMessage()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted(() => "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");

			RemoveTraceListener();
		}
	}
}