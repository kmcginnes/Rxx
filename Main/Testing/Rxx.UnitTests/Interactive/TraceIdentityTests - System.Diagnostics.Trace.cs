using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Interactive
{
	[TestClass]
	public partial class TraceIdentityTests : RxxTraceTests
	{
		[TestMethod]
		public void IxTestTraceIdentity()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnNext()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnNextFormat()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnNextLazyMessage()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnError()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError().Catch(Enumerable.Empty<int>()).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(id, ex));

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceIdentityOnErrorFormat()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError("OnError: {0}={1}").Catch(Enumerable.Empty<int>()).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.ToString());

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceIdentityOnErrorLazyMessage()
		{
			AddTraceListener();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError((oId, error) => "OnError: " + oId + "=" + error.Message).Catch(Enumerable.Empty<int>()).Run();

				int id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.Message);

				Listener.Clear();
			}

			RemoveTraceListener();
		}

		[TestMethod]
		public void IxTestTraceIdentityOnCompleted()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnCompletedFormat()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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
		public void IxTestTraceIdentityOnCompletedLazyMessage()
		{
			AddTraceListener();

			var xs = Enumerable.Range(0, 5);

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