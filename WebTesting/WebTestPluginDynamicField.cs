using Fiddler;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiddler.WebTesting
{
	public abstract class WebTestPluginDynamicField : IFiddlerWebTestPlugin
	{
		private const string Hidden = "$HIDDEN";

		private string m_Field;

		public string Field
		{
			get
			{
				return this.m_Field;
			}
			set
			{
				this.m_Field = value;
			}
		}

		protected WebTestPluginDynamicField()
		{
		}

		public void PreWebTestSave(object sender, PreWebTestSaveEventArgs e)
		{
			int ruleAddedCounter = 1;
			for (int i = 0; i < e.FiddlerWebTest.Sessions.Count; i++)
			{
				WebTestSession session = e.FiddlerWebTest.Sessions[i];
				if (session.FiddlerSession.requestBodyBytes != null && (long)session.FiddlerSession.requestBodyBytes.Length > (long)0)
				{
					FormPostParameter param = session.RequestFormParams.GetParameter(this.Field);
					if (param != null)
					{
						int j = i - 1;
						while (j >= 0)
						{
							WebTestSession prevSession = e.FiddlerWebTest.Sessions[j];
							session.FiddlerSession.utilDecodeResponse();
							string responseString = CONFIG.oHeaderEncoding.GetString(prevSession.FiddlerSession.responseBodyBytes).Trim();
							if (!responseString.Contains(param.Name) || !responseString.Contains(param.Value))
							{
								j--;
							}
							else
							{
								while (prevSession.ParentSession != null)
								{
									prevSession = prevSession.ParentSession;
								}
								if (prevSession.FiddlerSession.oRequest.headers.ExistsAndContains("x-microsoftajax", "Delta=true"))
								{
									ExtractionRule rule = new ExtractionRule("Microsoft.VisualStudio.TestTools.WebTesting.Rules.ExtractText, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
									{
										ContextParameterName = string.Concat("$HIDDEN", ruleAddedCounter.ToString(), ".", this.Field)
									};
									rule.Properties.Add(new RuleProperty("StartsWith", string.Concat(this.Field, "|")));
									rule.Properties.Add(new RuleProperty("EndsWith", "|"));
									rule.Properties.Add(new RuleProperty("IgnoreCase", "True"));
									rule.Properties.Add(new RuleProperty("UseRegularExpression", "False"));
									rule.Properties.Add(new RuleProperty("Required", "True"));
									rule.Properties.Add(new RuleProperty("Index", "0"));
									prevSession.ExtractionRules.Add(rule);
									param.Value = string.Concat(new string[] { "{{$HIDDEN", ruleAddedCounter.ToString(), ".", this.Field, "}}" });
								}
								else
								{
									ExtractionRule rule = new ExtractionRule("Microsoft.VisualStudio.TestTools.WebTesting.Rules.ExtractFormField, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
									{
										ContextParameterName = string.Concat("$HIDDEN", ruleAddedCounter.ToString(), ".", this.Field)
									};
									rule.Properties.Add(new RuleProperty("Name", this.Field));
									prevSession.ExtractionRules.Add(rule);
									param.Value = string.Concat(new string[] { "{{$HIDDEN", ruleAddedCounter.ToString(), ".", this.Field, "}}" });
								}
								ruleAddedCounter++;
								break;
							}
						}
					}
				}
			}
		}
	}
}