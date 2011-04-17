using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Dynamic Observable")]
	[Description("Using ObservableDynamicObject to wrap a normal object's "
						 + "properties, events and methods into observables.")]
	public sealed class ObservableDynamicObjectLab : RxxLab
	{
		private sealed class Poco
		{
			private string message;
			public string Message
			{
				get
				{
					return message;
				}
				set
				{
					message = value;

					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

					OnMessageChanged(EventArgs.Empty);
				}
			}

			public event EventHandler MessageChanged;
			private void OnMessageChanged(EventArgs e)
			{
				var messageChanged = MessageChanged;

				if (messageChanged != null)
					messageChanged(this, e);
			}

			public void Ping(int value)
			{
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

				OnPong(new PongEventArgs(value));
			}

			public event EventHandler<PongEventArgs> Pong;
			private void OnPong(PongEventArgs e)
			{
				var pong = Pong;

				if (pong != null)
					pong(this, e);
			}

			public int Calculate(string value)
			{
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

				return value.GetHashCode();
			}
		}

		private sealed class PongEventArgs : EventArgs
		{
			public int Value { get; private set; }

			public PongEventArgs(int value)
			{
				this.Value = value;
			}
		}

		protected override void Main()
		{
			TraceLine(Instructions.PressAnyKeyToCancel);

			// Start with a typical object...
			Poco obj = new Poco();

			// and wrap it with a dynamic proxy.
			dynamic asyncObj = ObservableDynamicObject.Create(obj);

			// Properties, events and methods will now return observables.

			IObservable<string> messageChanged = asyncObj.Message;

			IObservable<IEvent<PongEventArgs>> pongRaised = asyncObj.Pong;

			IObservable<int> pong = pongRaised.Select(e => e.EventArgs.Value);

			using (messageChanged.Subscribe(ConsoleOutput("Message changed")))
			using (pong.Subscribe(ConsoleOutput("Pong")))
			{
				Console.WriteLine(Text.ObservableDynamicObjectLabAssigningMessage);

				asyncObj.Message = "New Message";

				Console.WriteLine(Text.ObservableDynamicObjectLabCallingPing);

				IObservable<Unit> ping = asyncObj.Ping(12345);

				Console.WriteLine(Text.ObservableDynamicObjectLabCallingCalculate);

				IObservable<int> calculate = asyncObj.Calculate("Hello World");

				using (ping.Subscribe(ConsoleOutput("Ping")))
				using (calculate.Subscribe(ConsoleOutput("Calculate")))
				{
					Console.ReadKey();
				}
			}
		}
	}
}
