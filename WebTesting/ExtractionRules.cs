using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class ExtractionRules : Collection<ExtractionRule>
	{
		public ExtractionRules()
		{
		}

		public bool Contains(string Classname)
		{
			bool flag;
			using (IEnumerator<ExtractionRule> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!string.Equals(enumerator.Current.Classname, Classname))
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			return flag;
		}
	}
}