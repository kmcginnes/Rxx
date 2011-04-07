using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Reactive
{
	[TestClass]
	public partial class TraceIdentityTests : RxxTraceTests
	{
		[TestMethod]
		public void RxTestTraceIdentity()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentity().Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Concat(
					Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)),
					TraceDefaults.DefaultOnCompleted(id)));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnNext()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext().Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnNextFormat()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext("OnNext: {0}={1}").Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnNextLazyMessage()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext((oId, value) => "OnNext: " + oId + "=" + value).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnError()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError().Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(id, ex));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnErrorFormat()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError("OnError: {0}={1}").Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.ToString());

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnErrorLazyMessage()
		{
			AddTraceListener();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError((oId, error) => "OnError: " + oId + "=" + error.Message).Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.Message);

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnCompleted()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted().Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted(id));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnCompletedFormat()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted("OnCompleted: {0}").Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceIdentityOnCompletedLazyMessage()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(oId => "OnCompleted: " + oId).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}

			RemoveTraceListener();
		}
	}
}