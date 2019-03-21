using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class FormPostParameterCollection : Collection<FormPostParameter>
	{
		public FormPostParameterCollection()
		{
		}

		public bool Contains(string Name)
		{
			bool flag;
			using (IEnumerator<FormPostParameter> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!string.Equals(enumerator.Current.Name, Name))
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

		public FormPostParameter GetParameter(string Name)
		{
			FormPostParameter formPostParameter;
			using (IEnumerator<FormPostParameter> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FormPostParameter param = enumerator.Current;
					if (!string.Equals(param.Name, Name))
					{
						continue;
					}
					formPostParameter = param;
					return formPostParameter;
				}
				return null;
			}
			return formPostParameter;
		}
	}
}