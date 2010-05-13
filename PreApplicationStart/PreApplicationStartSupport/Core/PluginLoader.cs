using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace PreApplicationStartSupport.Core
{
	public static class PluginLoader
	{
		public static Assembly LoadPlugin (string baseName)
		{
			if (String.IsNullOrEmpty (baseName))
				throw new ArgumentException ("Value must not be null or empty.", "baseName");

			string pluginsRelDir = WebConfigurationManager.AppSettings ["pluginsDir"];
			if (String.IsNullOrEmpty (pluginsRelDir))
				pluginsRelDir = "plugins/";

			string appPhysicalDir = HttpRuntime.AppDomainAppPath;
			string pluginsDir = Path.Combine (appPhysicalDir, pluginsRelDir);
			if (String.Compare (pluginsDir, appPhysicalDir, StringComparison.OrdinalIgnoreCase) == 0) {
				MessageContainer.Instance.Add ("Plugins directory must not be the same as application root.");
				return null;
			}

			if (!pluginsDir.StartsWith (appPhysicalDir, StringComparison.OrdinalIgnoreCase)) {
				MessageContainer.Instance.Add ("Plugins directory '{0}' is not a subdirectory of application root.", pluginsDir);
				return null;
			}

			if (!Directory.Exists (pluginsDir)) {
				MessageContainer.Instance.Add ("Plugins directory '{0}' does not exist.", pluginsDir);
				return null;
			}

			string assemblyPath = Path.Combine (pluginsDir, baseName + ".dll");
			if (!File.Exists (assemblyPath)) {
				MessageContainer.Instance.Add ("Plugin '{0}' not found.", baseName);
				return null;
			}

			Assembly ret;
			try {
				ret = Assembly.LoadFrom (assemblyPath);
			} catch (Exception ex) {
				MessageContainer.Instance.Add ("Attempt to load plugin '{0}' failed because of exception '{1}' with message: {2}",
					baseName, ex.GetType (), ex.Message);
				return null;
			}

			MessageContainer.Instance.Add ("Plugin '{0}' loaded successfully.", baseName);
			return ret;
		}
	}
}
