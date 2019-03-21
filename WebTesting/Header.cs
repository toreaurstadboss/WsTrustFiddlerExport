using System;

namespace Fiddler.WebTesting
{
	public class Header
	{
		private string m_name;

		private string m_value;

		private bool m_writeToWebTest = true;

		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		public string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		public bool WriteToWebTest
		{
			get
			{
				return this.m_writeToWebTest;
			}
			set
			{
				this.m_writeToWebTest = value;
			}
		}

		public Header()
		{
		}
	}
}