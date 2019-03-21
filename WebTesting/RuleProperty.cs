using System;

namespace Fiddler.WebTesting
{
	public class RuleProperty
	{
		private string m_Name;

		private string m_Value;

		public string Name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		public string Value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		public RuleProperty(string Name, string Value)
		{
			this.m_Name = Name;
			this.m_Value = Value;
		}
	}
}