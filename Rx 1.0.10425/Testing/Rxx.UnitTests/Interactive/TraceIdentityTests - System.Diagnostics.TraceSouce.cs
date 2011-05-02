using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rxx.UnitTests.Interactive
{
	public partial class TraceIdentityTests : RxxTraceTests
	{
		[TestMethod]
		public void IxTestTraceSourceIdentity()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentity(source).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, Concat(
					Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)),
					TraceDefaults.DefaultOnCompleted(id)));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnNext()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => TraceDefaults.DefaultOnNext(id, value)));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnNextFormat()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source, "OnNext: {0}={1}").Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnNextLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnNext(source, (oId, value) => "OnNext: " + oId + "=" + value).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, Enumerable.Range(0, 5).Select(value => "OnNext: " + id + "=" + value));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnError()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source).Catch(Enumerable.Empty<int>()).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnError(id, ex));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnErrorFormat()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source, "OnError: {0}={1}").Catch(Enumerable.Empty<int>()).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.ToString());

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnErrorLazyMessage()
		{
			var source = CreateTraceSource();

			var ex = new RxxMockException("Error");
			var xs = EnumerableEx.Throw<int>(ex);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnError(source, (oId, error) => "OnError: " + oId + "=" + error.Message).Catch(Enumerable.Empty<int>()).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnError: " + id + "=" + ex.Message);

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnCompleted()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, TraceDefaults.DefaultOnCompleted(id));

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnCompletedFormat()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source, "OnCompleted: {0}").Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}
		}

		[TestMethod]
		public void IxTestTraceSourceIdentityOnCompletedLazyMessage()
		{
			var source = CreateTraceSource();

			var xs = Enumerable.Range(0, 5);

			for (int i = 0; i < 3; i++)
			{
				xs.TraceIdentityOnCompleted(source, oId => "OnCompleted: " + oId).Run();

				string id = GetCurrentId();

				AssertEqual(Listener.Messages, "OnCompleted: " + id);

				Listener.Clear();
			}
		}
	}
}