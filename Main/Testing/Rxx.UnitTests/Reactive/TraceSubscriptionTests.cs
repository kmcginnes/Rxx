using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rxx.Properties;

namespace Rxx.UnitTests.Reactive
{
	[TestClass]
	public class TraceSubscriptionTests : RxxTraceTests
	{
		[TestMethod]
		public void RxTestTraceSubscriptions()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions();

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					Text.DefaultSubscribingMessage, 
					Text.DefaultSubscribedMessage, 
					Text.DefaultDisposingSubscriptionMessage, 
					Text.DefaultDisposedSubscriptionMessage
				}
				.Repeat(iterations));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceSubscriptionsNamed()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions("Test Sequence");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					string.Format(Rxx.Properties.Text.SubscribingFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.SubscribedFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.DisposingSubscriptionFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.DisposedSubscriptionFormat, "Test Sequence")
				}
				.Repeat(iterations));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceSubscriptionsConnectMessage()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions("First", "Second");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					"First", 
					"Second", 
					Text.DefaultDisposingSubscriptionMessage, 
					Text.DefaultDisposedSubscriptionMessage
				}
				.Repeat(iterations));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceSubscriptionsLifetimeMessages()
		{
			AddTraceListener();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions("First", "Second", "Third", "Fourth");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					"First", 
					"Second", 
					"Third", 
					"Fourth"
				}
				.Repeat(iterations));

			RemoveTraceListener();
		}

		[TestMethod]
		public void RxTestTraceSourceSubscriptions()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions(source);

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					Text.DefaultSubscribingMessage, 
					Text.DefaultSubscribedMessage, 
					Text.DefaultDisposingSubscriptionMessage, 
					Text.DefaultDisposedSubscriptionMessage
				}
				.Repeat(iterations));
		}

		[TestMethod]
		public void RxTestTraceSourceSubscriptionsNamed()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions(source, "Test Sequence");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					string.Format(Rxx.Properties.Text.SubscribingFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.SubscribedFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.DisposingSubscriptionFormat, "Test Sequence"),
					string.Format(Rxx.Properties.Text.DisposedSubscriptionFormat, "Test Sequence")
				}
				.Repeat(iterations));
		}

		[TestMethod]
		public void RxTestTraceSourceSubscriptionsConnectMessage()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions(source, "First", "Second");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					"First", 
					"Second", 
					Text.DefaultDisposingSubscriptionMessage, 
					Text.DefaultDisposedSubscriptionMessage
				}
				.Repeat(iterations));
		}

		[TestMethod]
		public void RxTestTraceSourceSubscriptionsLifetimeMessages()
		{
			var source = CreateTraceSource();

			var xs = Observable.Range(0, 5)
				.TraceSubscriptions(source, "First", "Second", "Third", "Fourth");

			const int iterations = 3;

			xs.Repeat(iterations).Run();

			AssertEqual(Listener.Messages, new[]
				{
					"First", 
					"Second", 
					"Third", 
					"Fourth"
				}
				.Repeat(iterations));
		}
	}
}