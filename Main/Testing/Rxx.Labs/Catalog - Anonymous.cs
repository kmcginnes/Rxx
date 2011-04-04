using System;
using System.Collections.Generic;
using System.Disposables;
using System.Linq;
using DaveSexton.Labs;

namespace Rxx.Labs
{
	internal sealed partial class RxxLabCatalog : LabCatalog
	{
		/* Anonymous experiments (optional)
		 * 
		 * Enable this anonymous lab and it will be executed before all of 
		 * the discovered labs, including the labs specified above.
		 */
		private const bool anonymousEnabled = false;

		private static void Anonymous()
		{
			// Define anonymous experiments here and set enabled: true
		}
	}
}
