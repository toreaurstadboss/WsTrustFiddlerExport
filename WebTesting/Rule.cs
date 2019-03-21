using System;

namespace Fiddler.WebTesting
{
	public abstract class Rule
	{
		private string m_Classname;

		private RuleProperties m_Properties = new RuleProperties();

		public string Classname
		{
			get
			{
				return this.m_Classname;
			}
		}

		public RuleProperties Properties
		{
			get
			{
				return this.m_Properties;
			}
		}

		protected Rule(string Classname)
		{
			this.m_Classname = Classname;
		}
	}
}