using System;
using System.Reflection;

namespace Fiddler.WebTesting
{
	public class PluginClassReference
	{
		private PluginClassInfo m_PluginInfo;

		private ConstructorInfo m_Constructor;

		private Type m_Type;

		public PluginClassInfo PluginInfo
		{
			get
			{
				return this.m_PluginInfo;
			}
			set
			{
				this.m_PluginInfo = value;
			}
		}

		public PluginClassReference(PluginClassInfo PluginInfo)
		{
			this.m_PluginInfo = PluginInfo;
		}

		public IFiddlerWebTestPlugin CreateInstance()
		{
			IFiddlerWebTestPlugin fiddlerWebTestPlugin;
			try
			{
				if (this.m_Constructor == null)
				{
					this.m_Constructor = this.GetPluginType().GetConstructor(Type.EmptyTypes);
				}
				fiddlerWebTestPlugin = (IFiddlerWebTestPlugin)this.m_Constructor.Invoke(null);
			}
			catch
			{
				throw;
			}
			return fiddlerWebTestPlugin;
		}

		internal Type GetPluginType()
		{
			if (this.m_Type == null)
			{
				Assembly assembly = Assembly.LoadFile(this.m_PluginInfo.AssemblyLocation);
				this.m_Type = assembly.GetType(string.Concat(this.m_PluginInfo.Namespace, ".", this.m_PluginInfo.Classname), true);
			}
			return this.m_Type;
		}

		public override string ToString()
		{
			if (this.m_PluginInfo.Namespace == null || this.m_PluginInfo.Namespace.Length <= 0)
			{
				return this.m_PluginInfo.Classname;
			}
			return string.Concat(this.m_PluginInfo.Namespace, ".", this.m_PluginInfo.Classname);
		}
	}
}