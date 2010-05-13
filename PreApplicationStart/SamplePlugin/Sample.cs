using System;
using System.Collections.Generic;
using System.Reflection;

using PreApplicationStartSupport.Core;

namespace SamplePlugin
{
	public class Sample : IPlugin
	{
		#region IPlugin Members

		public string Name {
			get { return "Sample Plugin"; }
		}

		public Version Version { get; private set; }

		public void Initialize ()
		{
			Assembly asm = Assembly.GetExecutingAssembly ();
			object [] attributes = asm.GetCustomAttributes (typeof (AssemblyVersionAttribute), false);
			if (attributes == null || attributes.Length == 0) {
				Version = new Version (0, 0, 0, 0);
				return;
			}

			var asmVersion = attributes [0] as AssemblyVersionAttribute;
			Version = new Version (asmVersion.Version);
		}

		#endregion
	}
}
