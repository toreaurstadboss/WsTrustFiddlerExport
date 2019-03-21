using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class QueryStringParameterCollection : Collection<QueryStringParameter>
	{
		public QueryStringParameterCollection()
		{
		}

		public bool Contains(string Name)
		{
			bool flag;
			using (IEnumerator<QueryStringParameter> enumerator = base.GetEnumerator())
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

		public QueryStringParameter GetParameter(string Name)
		{
			QueryStringParameter queryStringParameter;
			using (IEnumerator<QueryStringParameter> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QueryStringParameter param = enumerator.Current;
					if (!string.Equals(param.Name, Name))
					{
						continue;
					}
					queryStringParameter = param;
					return queryStringParameter;
				}
				return null;
			}
			return queryStringParameter;
		}
	}
}