using System;

namespace Fiddler.WebTesting
{
	public abstract class Parameter
	{
		private string m_Name;

		private string m_Value;

		private bool m_UrlEncode = true;

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

		public bool UrlEncode
		{
			get
			{
				return this.m_UrlEncode;
			}
			set
			{
				this.m_UrlEncode = value;
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

		protected Parameter()
		{
		}
	}
}