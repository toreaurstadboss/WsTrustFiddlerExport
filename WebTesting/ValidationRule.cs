using System;

namespace Fiddler.WebTesting
{
	public class ValidationRule : Rule
	{
		private ValidationRule.Level m_level;

		public ValidationRule.Level ValidationLevel
		{
			get
			{
				return this.m_level;
			}
			set
			{
				this.m_level = value;
			}
		}

		public ValidationRule(string Classname) : base(Classname)
		{
		}

		public enum Level
		{
			High,
			Medium,
			Low
		}
	}
}