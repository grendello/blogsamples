using System;
using System.IO;
using System.Reflection;
using PreApplicationStartSupport.Core;

namespace SetupPlugin
{
	public class Setup : ISetupPlugin
	{
		public Setup ()
		{
		}

		#region ISetupPlugin Members

		void CreateDirectory (string name, string dataDir, MessageContainer messages)
		{
			string newDir = Path.Combine (dataDir, name);
			if (!Directory.Exists (newDir)) {
				Directory.CreateDirectory (newDir);
				messages.Add (1, false, "* directory {0} created", newDir);
			}
		}

		public bool PerformSetupSteps (string rootDir)
		{
			MessageContainer messages = MessageContainer.Instance;
			if (!Directory.Exists (rootDir)) {
				messages.Add ("Setup: root directory '{0}' does not exist.", rootDir);
				return false;
			}

			string dataDir = Path.Combine (rootDir, "App_Data");
			if (!Directory.Exists (dataDir)) {
				messages.Add ("Setup: application data directory '{0}' does not exist.", dataDir);
				return false;
			}

			try {
				messages.Add ("Setup: creating directories");
				CreateDirectory ("directory1", dataDir, messages);
				CreateDirectory ("logs", dataDir, messages);
				CreateDirectory ("backups", dataDir, messages);

				File.WriteAllText (Path.Combine (dataDir, ".setup_done"), DateTime.Now.ToString ());
			} catch (Exception ex) {
				messages.Add ("Setup: creating directories failed with exception '{0}': {1}", ex.GetType (), ex.Message);
				return false;
			}

			return true;
		}

		#endregion

		#region IPlugin Members

		public string Name
		{
			get { return "Setup Plugin"; }
		}

		public Version Version { get; private set; }

		public void Initialize ()
		{
			Assembly asm = GetType ().Assembly;
			object[] attributes = asm.GetCustomAttributes (typeof (AssemblyVersionAttribute), false);
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
