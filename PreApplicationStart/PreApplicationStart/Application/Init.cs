using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;

using PreApplicationStartSupport.Core;

// There can be only one such attribute instance per assembly.
[assembly:PreApplicationStartMethod (typeof (PreApplicationStart.Application.Init), "InitializeApplication")]

namespace PreApplicationStart.Application
{
	public class Init
	{
		public static void InitializeApplication ()
		{
			string appDir = HttpRuntime.AppDomainAppPath;
			string stampFile = Path.Combine (appDir, "App_Data", ".setup_done");
			MessageContainer messages = MessageContainer.Instance;

			messages.Clear ();
			if (!File.Exists (stampFile)) {
				messages.Add ("First run - need to perform setup steps.");
				RunSetup (messages, appDir);
			}

			Assembly pluginAssembly = PluginLoader.LoadPlugin ("SamplePlugin");
			if (pluginAssembly == null)
				messages.Add ("Init: failed to load plugin 'SamplePlugin'");
			else
				LoadPluginResources (pluginAssembly, messages);
		}

		static void LoadPluginResources (Assembly pluginAssembly, MessageContainer messages)
		{
			Type [] types = pluginAssembly.GetTypes ();
			IPlugin plugin;

			foreach (Type t in types) {
				if (!typeof (IPlugin).IsAssignableFrom (t))
					continue;

				try {
					plugin = Activator.CreateInstance (t) as IPlugin;
					plugin.Initialize ();
					messages.Add ("Init: plugin type '{0}' v{1} loaded", t.FullName, plugin.Version);
				} catch (Exception ex) {
					messages.Add ("Init: failed to create instance of type '{0}'. Exception '{1}' was caught, with message: {2}",
						t.FullName, ex.GetType (), ex.Message);
					continue;
				}
			}
		}

		static void RunSetup (MessageContainer messages, string appDir)
		{
			Assembly setupAssembly = PluginLoader.LoadPlugin ("SetupPlugin");
			if (setupAssembly == null) {
				messages.Add ("Init: failed to load setup plugin assembly.");
				return;
			}

			Type[] types = setupAssembly.GetTypes ();
			ISetupPlugin setupPlugin;
			foreach (Type t in types) {
				if (!typeof (ISetupPlugin).IsAssignableFrom (t))
					continue;

				try {
					setupPlugin = Activator.CreateInstance (t) as ISetupPlugin;
					setupPlugin.Initialize ();
					messages.Add ("Init: setup type '{0}' v{1} loaded", t.FullName, setupPlugin.Version);
				} catch (Exception ex) {
					messages.Add ("Init: failed to create instance of type '{0}' - cannot perform setup with this type. Exception '{1}' was caught, with message: {2}",
						t.FullName, ex.GetType (), ex.Message);
					continue;
				}

				try {
					if (!setupPlugin.PerformSetupSteps (appDir)) {
						messages.Add ("Init: setup failed.");
						continue;
					}
					messages.Add ("Init: setup successful.");
				} catch (Exception ex) {
					messages.Add ("Init: setup failed. Exception '{0}' caught: {1}", ex.GetType (), ex.Message);
				}
			}
		}
	}
}