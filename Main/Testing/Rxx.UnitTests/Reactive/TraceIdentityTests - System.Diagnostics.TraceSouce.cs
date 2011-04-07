using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Reactive
{
	public partial class TraceIdentityTests : RxxTraceTests
	{
		[TestMethod]
		public void RxTestTraceSourceIdentity()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentity(source).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Concat(
					Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)),
					TraceDefaults.DefaultOnCompleted(id)));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnNext()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnNextFormat()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source, "OnNext: {0}={1}").Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnNextLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source, (oId, value) => "OnNext: " + oId + "=" + value).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnError()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source).Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(id, ex));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnErrorFormat()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source, "OnError: {0}={1}").Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.ToString());

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnErrorLazyMessage()
		{
			var source = CreateTraceSource();

			var ex = new Exception("Error");
			var xs = Observable.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source, (oId, error) => "OnError: " + oId + "=" + error.Message).Run(_ => { }, __ => { });

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.Message);

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnCompleted()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted(id));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnCompletedFormat()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source, "OnCompleted: {0}").Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}
		}

		[TestMethod]
		public void RxTestTraceSourceIdentityOnCompletedLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source, oId => "OnCompleted: " + oId).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}
		}
	}
}