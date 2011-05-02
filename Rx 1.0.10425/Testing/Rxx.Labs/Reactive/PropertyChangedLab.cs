﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Rxx.Labs.Properties;

namespace Rxx.Labs.Reactive
{
	[DisplayName("Property Changed Events")]
	[Description("Three different patterns of property changed events converted to "
						 + "observables using Observable2.FromPropertyChangedPattern.")]
	public sealed class PropertyChangedLab : RxxLab
	{
		private sealed class Foo : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private string v;
			public string Value
			{
				get
				{
					return v;
				}
				set
				{
					this.v = value;
					PropertyChanged(this, new PropertyChangedEventArgs("Value"));
				}
			}
		}

		private sealed class Bar
		{
			public event EventHandler ValueChanged;

			private string v;
			public string Value
			{
				get
				{
					return v;
				}
				set
				{
					this.v = value;
					ValueChanged(this, EventArgs.Empty);
				}
			}
		}

		private sealed class Baz : DependencyObject
		{
			public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
				"Value", typeof(string), typeof(Baz), new PropertyMetadata(null));

			public string Value
			{
				get
				{
					return (string) GetValue(ValueProperty);
				}
				set
				{
					SetValue(ValueProperty, value);
				}
			}
		}

		protected override void Main()
		{
			var foo = new Foo();
			var bar = new Bar();
			var baz = new Baz();

			var seeFooChange = Observable2.FromPropertyChangedPattern(() => foo.Value);
			var seeBarChange = Observable2.FromPropertyChangedPattern(() => bar.Value);
			var seeBazChange = Observable2.FromPropertyChangedPattern(() => baz.Value);

			using (seeFooChange.Subscribe(ConsoleOutput))
			using (seeBarChange.Subscribe(ConsoleOutput))
			using (seeBazChange.Subscribe(ConsoleOutput))
			{
				foo.Value = UserInput(Text.PromptFormat, Instructions.EnterAValueForFoo);
				bar.Value = UserInput(Text.PromptFormat, Instructions.EnterAValueForBar);
				baz.Value = UserInput(Text.PromptFormat, Instructions.EnterAValueForBaz);
			}
		}
	}
}
