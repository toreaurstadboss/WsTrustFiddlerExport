using Fiddler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiddler.WebTesting
{
	public class WebTestSession
	{
		private Session m_session;

		private WebTestSession m_childSession;

		private WebTestSession m_parentSession;

		private FormPostParameterCollection m_requestFormParams;

		private QueryStringParameterCollection m_requestQueryParams;

		private HeaderCollection m_headers;

		private Fiddler.WebTesting.ExtractionRules m_requestExtractionRules;

		private Fiddler.WebTesting.ValidationRules m_requestValidationRules;

		private bool m_writeToWebTest = true;

		private bool m_cacheControl;

		private string m_encoding = "utf-8";

		private bool m_followRedirects;

		private bool m_parseDependentLinks = true;

		private bool m_recordResults = true;

		private int m_responseTimeGoal;

		private int m_timeout = 60;

		private int m_thinkTime = -1;

		public bool CacheControl
		{
			get
			{
				return this.m_cacheControl;
			}
			set
			{
				this.m_cacheControl = value;
			}
		}

		public WebTestSession ChildSession
		{
			get
			{
				return this.m_childSession;
			}
			set
			{
				this.m_childSession = value;
			}
		}

		public Fiddler.WebTesting.ExtractionRules ExtractionRules
		{
			get
			{
				if (this.m_requestExtractionRules == null)
				{
					this.m_requestExtractionRules = new Fiddler.WebTesting.ExtractionRules();
				}
				return this.m_requestExtractionRules;
			}
		}

		public Session FiddlerSession
		{
			get
			{
				return this.m_session;
			}
		}

		public bool FollowRedirects
		{
			get
			{
				return this.m_followRedirects;
			}
			set
			{
				this.m_followRedirects = value;
			}
		}

		public HeaderCollection Headers
		{
			get
			{
				if (this.m_headers == null)
				{
					Dictionary<string, bool> oIgnoreHeaders = this.GetListOfHeadersToIgnore();
					this.m_headers = new HeaderCollection();
					foreach (HTTPHeaderItem oHeader in this.FiddlerSession.oRequest.headers)
					{
						Header header = new Header();
						if (oIgnoreHeaders.ContainsKey(oHeader.Name))
						{
							header.WriteToWebTest = false;
						}
						header.Name = oHeader.Name;
						header.Value = oHeader.Value;
						this.m_headers.Add(header);
					}
				}
				return this.m_headers;
			}
		}

		public WebTestSession ParentSession
		{
			get
			{
				return this.m_parentSession;
			}
			set
			{
				this.m_parentSession = value;
			}
		}

		public bool ParseDependentRequests
		{
			get
			{
				return this.m_parseDependentLinks;
			}
			set
			{
				this.m_parseDependentLinks = value;
			}
		}

		public bool RecordResults
		{
			get
			{
				return this.m_recordResults;
			}
			set
			{
				this.m_recordResults = value;
			}
		}

		public FormPostParameterCollection RequestFormParams
		{
			get
			{
				if (this.m_requestFormParams == null)
				{
					this.m_requestFormParams = new FormPostParameterCollection();
					if (this.FiddlerSession.oRequest.headers.ExistsAndContains("Content-Type", "application/x-www-form-urlencoded"))
					{
						string[] strArrays = this.FiddlerSession.GetRequestBodyAsString().Trim().Split(new char[] { '&' });
						for (int i = 0; i < (int)strArrays.Length; i++)
						{
							string oNVP = strArrays[i];
							string name = oNVP;
							string value = string.Empty;
							if (oNVP.IndexOf("=") >= 0)
							{
								name = Utilities.UrlDecode(oNVP.Substring(0, oNVP.IndexOf("=")), Encoding.UTF8);
								string str = oNVP;
								value = Utilities.UrlDecode(str.Substring(str.IndexOf("=") + 1), Encoding.UTF8);
							}
							this.m_requestFormParams.Add(new FormPostParameter(name, value, true));
						}
					}
				}
				return this.m_requestFormParams;
			}
		}

		public QueryStringParameterCollection RequestQueryParams
		{
			get
			{
				if (this.m_requestQueryParams == null)
				{
					this.m_requestQueryParams = new QueryStringParameterCollection();
					int ixQueryPart = this.FiddlerSession.url.IndexOf("?");
					if (ixQueryPart > -1)
					{
						string sQueryString = this.FiddlerSession.url.Substring(ixQueryPart + 1);
						if (sQueryString != null && sQueryString.Length > 0)
						{
							string[] strArrays = sQueryString.Split(new char[] { '&' });
							for (int i = 0; i < (int)strArrays.Length; i++)
							{
								string oQSP = strArrays[i];
								if (oQSP.IndexOf("=") >= 0)
								{
									string name = Utilities.UrlDecode(oQSP.Substring(0, oQSP.IndexOf("=")));
									string str = oQSP;
									string value = Utilities.UrlDecode(str.Substring(str.IndexOf("=") + 1));
									this.m_requestQueryParams.Add(new QueryStringParameter(name, value, true));
								}
							}
						}
					}
				}
				return this.m_requestQueryParams;
			}
		}

		public int ResponseTimeGoal
		{
			get
			{
				return this.m_responseTimeGoal;
			}
			set
			{
				this.m_responseTimeGoal = value;
			}
		}

		public string TextEncoding
		{
			get
			{
				return this.m_encoding;
			}
			set
			{
				this.m_encoding = value;
			}
		}

		public int ThinkTime
		{
			get
			{
				return this.m_thinkTime;
			}
			set
			{
				this.m_thinkTime = value;
			}
		}

		public int Timeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_timeout = value;
			}
		}

		public Fiddler.WebTesting.ValidationRules ValidationRules
		{
			get
			{
				if (this.m_requestValidationRules == null)
				{
					this.m_requestValidationRules = new Fiddler.WebTesting.ValidationRules();
				}
				return this.m_requestValidationRules;
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

		public WebTestSession(Fiddler.Session Session)
		{
			this.m_session = Session;
		}

		private Dictionary<string, bool> GetListOfHeadersToIgnore()
		{
			return new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
			{
				{ "Content-Length", true },
				{ "Cookie", true },
				{ "Expect", true },
				{ "Proxy-Connection", true },
				{ "User-Agent", true },
				{ "Referer", true },
				{ "Accept", true },
				{ "Accept-Language", true },
				{ "Accept-Encoding", true },
				{ "Host", true },
				{ "Connection", true },
				{ "Cache-Control", true },
				{ "Authorization", true }
			};
		}
	}
}