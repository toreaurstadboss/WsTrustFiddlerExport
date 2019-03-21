using System;

namespace Fiddler.WebTesting
{
	public class QueryStringParameter : Parameter
	{
		private bool m_useToGroupResults;

		public bool UseToGroupResults
		{
			get
			{
				return this.m_useToGroupResults;
			}
			set
			{
				this.m_useToGroupResults = value;
			}
		}

		public QueryStringParameter(string Name, string Value)
		{
			base.Name = Name;
			base.Value = Value;
		}

		public QueryStringParameter(string Name, string Value, bool UrlEncode)
		{
			base.Name = Name;
			base.Value = Value;
			base.UrlEncode = UrlEncode;
		}
	}
}