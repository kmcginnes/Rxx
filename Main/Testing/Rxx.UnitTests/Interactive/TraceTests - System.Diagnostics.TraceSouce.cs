using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Interactive
{
	public partial class TraceTests : RxxTraceTests
	{
		[TestMethod]
		public void IxTestTraceSource()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.Trace(source).Run();

			AssertEqual(Listener.Messages, Concat(
				Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)),
				TraceDefaults.DefaultOnCompleted()));
		}

		[TestMethod]
		public void IxTestTraceSourceOnNext()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext(source).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(value)));
		}

		[TestMethod]
		public void IxTestTraceSourceOnNextFormat()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext(source, "OnNext: {0}").Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));
		}

		[TestMethod]
		public void IxTestTraceSourceOnNextLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnNext(source, value => "OnNext: " + value).Run();

			AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + value));
		}

		[TestMethod]
		public void IxTestTraceSourceOnError()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError(source).Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(ex));
		}

		[TestMethod]
		public void IxTestTraceSourceOnErrorFormat()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError(source, "OnError: {0}").Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, "OnError: " + ex.ToString());
		}

		[TestMethod]
		public void IxTestTraceSourceOnErrorLazyMessage()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			xs.TraceOnError(source, error => "OnError: " + error.Message).Catch(Enumerable.Empty<int>()).Run();

			AssertEqual(Listener.Messages, "OnError: " + ex.Message);
		}

		[TestMethod]
		public void IxTestTraceSourceOnCompleted()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted(source).Run();

			AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted());
		}

		[TestMethod]
		public void IxTestTraceSourceOnCompletedFormat()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted(source, "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");
		}

		[TestMethod]
		public void IxTestTraceSourceOnCompletedLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			xs.TraceOnCompleted(source, () => "OnCompleted").Run();

			AssertEqual(Listener.Messages, "OnCompleted");
		}
	}
}