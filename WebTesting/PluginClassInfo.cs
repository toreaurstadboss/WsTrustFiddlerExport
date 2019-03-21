using System;

namespace Fiddler.WebTesting
{
	public class PluginClassInfo
	{
		private string m_Classname;

		private string m_Assembly;

		private string m_Namespace;

		private string m_AssemblyLocation;

		public string Assembly
		{
			get
			{
				return this.m_Assembly;
			}
			set
			{
				this.m_Assembly = value;
			}
		}

		public string AssemblyLocation
		{
			get
			{
				return this.m_AssemblyLocation;
			}
			set
			{
				this.m_AssemblyLocation = value;
			}
		}

		public string Classname
		{
			get
			{
				return this.m_Classname;
			}
			set
			{
				this.m_Classname = value;
			}
		}

		public string Namespace
		{
			get
			{
				return this.m_Namespace;
			}
			set
			{
				this.m_Namespace = value;
			}
		}

		internal PluginClassInfo()
		{
		}

		public string CreateFullClassName()
		{
			string fullClassName = string.Empty;
			if (this.m_Namespace.Length > 0)
			{
				fullClassName = string.Concat(this.m_Namespace, ".");
			}
			fullClassName = string.Concat(fullClassName, this.m_Classname);
			if (this.m_Assembly.Length > 0)
			{
				fullClassName = string.Concat(fullClassName, ", ", this.m_Assembly);
			}
			return fullClassName;
		}
	}
}