using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace Fiddler.WebTesting
{
	internal class AssemblyHelper
	{
		private string m_Path;

		private List<PluginClassReference> m_plugins;

		public AssemblyHelper(string Path)
		{
			this.m_Path = Path;
		}

		public List<PluginClassReference> FindAvailablePlugins()
		{
			Assembly assembly;
			this.m_plugins = new List<PluginClassReference>();
			bool bNoisyEvents = FiddlerApplication.Prefs.GetBoolPref("fiddler.debug.extensions.verbose", false);
			if (bNoisyEvents)
			{
				FiddlerApplication.Log.LogFormat("Searching for VSWebTest Export Plugins under: {0}", new object[] { this.m_Path });
			}
			try
			{
				if (Directory.Exists(this.m_Path))
				{
					Evidence evidenceFiddler = Assembly.GetExecutingAssembly().Evidence;
					FileInfo[] files = (new DirectoryInfo(this.m_Path)).GetFiles("*.dll");
					for (int i = 0; i < (int)files.Length; i++)
					{
						FileInfo assemblyInfo = files[i];
						if (!assemblyInfo.Name.StartsWith("_", StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								assembly = (Environment.Version.Major >= 4 ? Assembly.LoadFrom(assemblyInfo.FullName) : Assembly.LoadFrom(assemblyInfo.FullName, evidenceFiddler));
								this.FindPluginClassesInAssembly(assembly);
							}
							catch (Exception exception)
							{
								Exception eX = exception;
								if (bNoisyEvents)
								{
									FiddlerApplication.Log.LogFormat("Failed to load WebTestPlugin {0}; exception was {1}", new object[] { assemblyInfo.FullName, eX });
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				FiddlerApplication.ReportException(exception1, "WebTestPlugin Enumeration Failed");
			}
			this.FindPluginClassesInAssembly(Assembly.GetCallingAssembly());
			return this.m_plugins;
		}

		private void FindPluginClassesInAssembly(Assembly assembly)
		{
			try
			{
				if (FiddlerApplication.Prefs.GetBoolPref("fiddler.debug.extensions.verbose", false))
				{
					FiddlerApplication.Log.LogFormat("Scanning for WebTestPlugins in {0}.", new object[] { assembly.FullName });
				}
				Type[] exportedTypes = assembly.GetExportedTypes();
				for (int i = 0; i < (int)exportedTypes.Length; i++)
				{
					Type oType = exportedTypes[i];
					if (!oType.IsAbstract && oType.IsPublic && oType.IsClass && typeof(IFiddlerWebTestPlugin).IsAssignableFrom(oType))
					{
						if (FiddlerApplication.Prefs.GetBoolPref("fiddler.debug.extensions.verbose", false))
						{
							FiddlerApplication.Log.LogFormat("WebTestPlugin found: {0}.", new object[] { oType.FullName });
						}
						PluginClassInfo pluginInfo = new PluginClassInfo()
						{
							Assembly = assembly.FullName,
							Classname = oType.Name,
							Namespace = oType.Namespace,
							AssemblyLocation = assembly.Location
						};
						this.m_plugins.Add(new PluginClassReference(pluginInfo));
					}
				}
			}
			catch (Exception exception)
			{
				Exception eX = exception;
				if (FiddlerApplication.Prefs.GetBoolPref("fiddler.debug.extensions.verbose", false))
				{
					FiddlerApplication.Log.LogFormat("Failed to load {0} to find plugin classes; exception was {1}", new object[] { assembly.FullName, eX });
				}
			}
		}
	}
}