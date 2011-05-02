using System;
using DaveSexton.Labs;

namespace Rxx.Labs
{
	internal static class Program
	{
		// Use the Catalog.cs file to configure labs.

		private static void Main()
		{
			Console.CancelKeyPress += (sender, e) => e.Cancel = true;

			using (var controller = new ConsoleLabController(new RxxLabCatalog()))
			{
#if DEBUG
				controller.StartDebug();
#else
				controller.Start();
#endif
			}
		}
	}
}
