using System;

namespace Fiddler.WebTesting
{
	public class FormPostParameter : Parameter
	{
		public FormPostParameter(string Name, string Value)
		{
			base.Name = Name;
			base.Value = Value;
		}

		public FormPostParameter(string Name, string Value, bool UrlEncode)
		{
			base.Name = Name;
			base.Value = Value;
			base.UrlEncode = UrlEncode;
		}
	}
}