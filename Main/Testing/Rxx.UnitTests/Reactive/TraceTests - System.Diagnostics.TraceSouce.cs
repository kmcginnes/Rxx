using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Reactive
{
	public partial class TraceTests : RxxTraceTests
	{
		[TestMethod]
		public void RxTestTraceSource()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.Trace(source).Run();

			AssertEqual(Listener.Messages, Concat(
				Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)),
				TraceDefaults.DefaultOnCompleted()));
		}

		[TestMethod]
		public void RxTestTraceSourceOnNext()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext(source).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)));
		}

		[TestMethod]
		public void RxTestTraceSourceOnNextFormat()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext(source, "OnNext: {0}").Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));
		}

		[TestMethod]
		public void RxTestTraceSourceOnNextLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnNext(source, value => "OnNext: " + value).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));
		}

		[TestMethod]
		public void RxTestTraceSourceOnError()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError(source).Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(ex));
		}

		[TestMethod]
		public void RxTestTraceSourceOnErrorFormat()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError(source, "OnError: {0}").Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, "OnError: " + ex.ToString());
		}

		[TestMethod]
		public void RxTestTraceSourceOnErrorLazyMessage()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			xs.TraceOnError(source, error => "OnError: " + error.Message).Run(_ => { }, __ => { });

			AssertEqual(Listener.Messages, "OnError: " + ex.Message);
		}

		[TestMethod]
		public void RxTestTraceSourceOnCompleted()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted(source).Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted());
		}

		[TestMethod]
		public void RxTestTraceSourceOnCompletedFormat()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted(source, "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");
		}

		[TestMethod]
		public void RxTestTraceSourceOnCompletedLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			xs.TraceOnCompleted(source, () => "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");
		}
	}
}