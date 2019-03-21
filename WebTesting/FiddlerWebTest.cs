using Fiddler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Fiddler.WebTesting
{
    public class FiddlerWebTest
    {
        private Session[] m_Sessions;

        private WebTestSessionCollection m_FiddlerSessions;

        private string m_userName = string.Empty;

        private string m_password = string.Empty;

        private bool m_preAuthenticate = true;

        private string m_proxy = string.Empty;

        public string Password
        {
            get
            {
                return this.m_password;
            }
            set
            {
                this.m_password = value;
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                return this.m_preAuthenticate;
            }
            set
            {
                this.m_preAuthenticate = value;
            }
        }

        public string Proxy
        {
            get
            {
                return this.m_proxy;
            }
            set
            {
                this.m_proxy = value;
            }
        }

        public WebTestSessionCollection Sessions
        {
            get
            {
                if (this.m_FiddlerSessions == null)
                {
                    this.m_FiddlerSessions = new WebTestSessionCollection();
                    Session[] mSessions = this.m_Sessions;
                    for (int i = 0; i < (int)mSessions.Length; i++)
                    {
                        Session oSession = mSessions[i];
                        this.m_FiddlerSessions.Add(new WebTestSession(oSession));
                    }
                }
                return this.m_FiddlerSessions;
            }
        }

        public string UserName
        {
            get
            {
                return this.m_userName;
            }
            set
            {
                this.m_userName = value;
            }
        }

        public FiddlerWebTest(Session[] inSessions)
        {
            this.m_Sessions = inSessions;
        }

        private static void _WriteSessionComment(Session oSession, XmlTextWriter oXML, bool bIncludeAutoGeneratedComments)
        {
            if (!oSession.oFlags.ContainsKey("ui-comments"))
            {
                return;
            }
            string sComment = oSession.oFlags["ui-comments"].Trim();
            if (Utilities.IsCommentUserSupplied(sComment) || !string.IsNullOrEmpty(sComment) & bIncludeAutoGeneratedComments)
            {
                oXML.WriteStartElement("Comment");
                oXML.WriteAttributeString("CommentText", sComment);
                oXML.WriteEndElement();
            }
        }

        private void ExecutePlugin(StringBuilder errors, EventHandler<PreWebTestSaveEventArgs> sessionHandler)
        {
            PreWebTestSaveEventArgs e = new PreWebTestSaveEventArgs(this);
            try
            {
                sessionHandler(this, e);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                if (errors.Length == 0)
                {
                    errors.AppendLine("One or more plugins returned an error, but the remaining plugins executed and the WebTest was written.\n\n---\n");
                }
                errors.AppendLine(ex.Message);
            }
        }

        private void InvokeHandler(StringBuilder errors, Delegate[] sessionHandlers, Type typeToMatch)
        {
            Delegate[] delegateArray = sessionHandlers;
            for (int i = 0; i < (int)delegateArray.Length; i++)
            {
                EventHandler<PreWebTestSaveEventArgs> sessionHandler = (EventHandler<PreWebTestSaveEventArgs>)delegateArray[i];
                if (sessionHandler.Method.DeclaringType.Equals(typeToMatch))
                {
                    this.ExecutePlugin(errors, sessionHandler);
                    return;
                }
            }
        }

        internal void InvokePreWebTestSave()
        {
            if (this.PreWebTestSave == null)
            {
                return;
            }
            List<Type> orderedHandlers = new List<Type>()
            {
                typeof(FilterAuthRequests),
                typeof(FilterRedirects)
            };
            Delegate[] sessionHandlers = this.PreWebTestSave.GetInvocationList();
            StringBuilder errors = new StringBuilder();
            foreach (Type type in orderedHandlers)
            {
                this.InvokeHandler(errors, sessionHandlers, type);
            }
            Delegate[] delegateArray = sessionHandlers;
            for (int i = 0; i < (int)delegateArray.Length; i++)
            {
                EventHandler<PreWebTestSaveEventArgs> sessionHandler = (EventHandler<PreWebTestSaveEventArgs>)delegateArray[i];
                if (!orderedHandlers.Contains(sessionHandler.Method.DeclaringType))
                {
                    this.ExecutePlugin(errors, sessionHandler);
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Error during save...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static bool IsIgnoredRequest(Session oSession)
        {
            if (oSession.HTTPMethodIs("CONNECT"))
            {
                return true;
            }
            if (oSession.isTunnel)
            {
                string sLen = oSession.oRequest["Content-Length"];
                if (sLen != null && sLen != string.Empty && sLen != "0")
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadPlugins(List<PluginClassReference> PluginReferences)
        {
            foreach (PluginClassReference pluginReference in PluginReferences)
            {
                IFiddlerWebTestPlugin fiddlerWebTestPlugin = pluginReference.CreateInstance();
                this.PreWebTestSave += new EventHandler<PreWebTestSaveEventArgs>(fiddlerWebTestPlugin.PreWebTestSave);
            }
        }

        public void Save(string Path, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            this.Save(Path, evtProgressNotifications, true);
        }

        public void Save(string Path, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications, bool bIncludeAutoGeneratedComments)
        {
            string sBaseURL;
            int timeout;
            FiddlerWebTest.BodyType btBodyEncoding;
            try
            {
                this.InvokePreWebTestSave();
                bool usingNex = false;
                XmlTextWriter oXML = new XmlTextWriter(Path, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented
                };
                oXML.WriteStartDocument();
                oXML.WriteStartElement("TestCase");
                oXML.WriteAttributeString("Name", "FiddlerGeneratedWebTest");
                oXML.WriteAttributeString("Id", string.Empty);
                oXML.WriteAttributeString("Owner", string.Empty);
                oXML.WriteAttributeString("Description", string.Empty);
                oXML.WriteAttributeString("Priority", "0");
                oXML.WriteAttributeString("Enabled", "True");
                oXML.WriteAttributeString("CssProjectStructure", string.Empty);
                oXML.WriteAttributeString("CssIteration", string.Empty);
                oXML.WriteAttributeString("DeploymentItemsEditable", string.Empty);
                oXML.WriteAttributeString("CredentialUserName", this.UserName);
                oXML.WriteAttributeString("CredentialPassword", this.Password);
                bool preAuthenticate = this.PreAuthenticate;
                oXML.WriteAttributeString("PreAuthenticate", preAuthenticate.ToString());
                oXML.WriteAttributeString("Proxy", this.Proxy);
                oXML.WriteAttributeString("RequestCallbackClass", string.Empty);
                oXML.WriteAttributeString("TestCaseCallbackClass", string.Empty);
                oXML.WriteStartElement("Items");
                int i = 0;
                while (i < this.Sessions.Count)
                {
                    Session m_session = this.Sessions[i].FiddlerSession;
                    if (!m_session.oFlags.ContainsKey("neXpert.Step") || !(m_session["neXpert.Step"] != string.Empty))
                    {
                        i++;
                    }
                    else
                    {
                        usingNex = true;
                        break;
                    }
                }
                string m_name = "Transaction1";
                if (usingNex)
                {
                    oXML.WriteStartElement("TransactionTimer");
                    i = 0;
                    while (i < this.Sessions.Count)
                    {
                        Session m_session = this.Sessions[i].FiddlerSession;
                        if (!m_session.oFlags.ContainsKey("neXpert.Step") || !(m_session["neXpert.Step"] != string.Empty))
                        {
                            i++;
                        }
                        else
                        {
                            m_name = m_session["neXpert.Step"];
                            break;
                        }
                    }
                    oXML.WriteAttributeString("Name", m_name);
                    oXML.WriteStartElement("Items");
                }
                bool newTrans = false;
                for (i = 0; i < this.Sessions.Count; i++)
                {
                    WebTestSession webTestSession = this.Sessions[i];
                    Session oSession = webTestSession.FiddlerSession;
                    if (usingNex)
                    {
                        if (newTrans)
                        {
                            oXML.WriteEndElement();
                            oXML.WriteEndElement();
                            m_name = string.Empty;
                            int j = i;
                            while (j < this.Sessions.Count)
                            {
                                Session m_session = this.Sessions[j].FiddlerSession;
                                if (!m_session.oFlags.ContainsKey("neXpert.Step") || !(m_session["neXpert.Step"] != string.Empty))
                                {
                                    j++;
                                }
                                else
                                {
                                    m_name = m_session["neXpert.Step"];
                                    break;
                                }
                            }
                            if (m_name == string.Empty)
                            {
                                m_name = "Transaction1";
                            }
                            oXML.WriteStartElement("TransactionTimer");
                            oXML.WriteAttributeString("Name", m_name);
                            oXML.WriteStartElement("Items");
                            newTrans = false;
                        }
                        if (oSession.oFlags.ContainsKey("neXpert.Step") && oSession["neXpert.Step"] != string.Empty)
                        {
                            newTrans = true;
                        }
                    }
                    if (!webTestSession.WriteToWebTest)
                    {
                        if (evtProgressNotifications != null)
                        {
                            evtProgressNotifications(null, new ProgressCallbackEventArgs((float)(i + 1) / (float)this.Sessions.Count, string.Concat("skipped writing ", i.ToString(), " to WebTest.")));
                        }
                        FiddlerWebTest._WriteSessionComment(oSession, oXML, false);
                    }
                    else if (oSession.oRequest == null || oSession.oRequest.headers == null)
                    {
                        if (evtProgressNotifications != null)
                        {
                            evtProgressNotifications(null, new ProgressCallbackEventArgs((float)(i + 1) / (float)this.Sessions.Count, string.Concat("Ignoring Session #", oSession.id, " for WebTest.")));
                        }
                        FiddlerWebTest._WriteSessionComment(oSession, oXML, false);
                    }
                    else if (!FiddlerWebTest.IsIgnoredRequest(oSession))
                    {
                        string thinkTime = "0";
                        int j = i + 1;
                        while (j < this.Sessions.Count)
                        {
                            if (!this.Sessions[j].WriteToWebTest)
                            {
                                j++;
                            }
                            else
                            {
                                TimeSpan thinkDiff = this.Sessions[j].FiddlerSession.Timers.ClientDoneRequest - webTestSession.FiddlerSession.Timers.ServerDoneResponse;
                                double think = Math.Round(thinkDiff.TotalSeconds, 0);
                                thinkTime = think.ToString();
                                break;
                            }
                        }
                        FiddlerWebTest._WriteSessionComment(oSession, oXML, bIncludeAutoGeneratedComments);
                        oXML.WriteStartElement("Request");
                        oXML.WriteAttributeString("Method", oSession.RequestMethod);
                        oXML.WriteAttributeString("Version", oSession.oRequest.headers.HTTPVersion.Substring(5));
                        string sReportingName = oSession["WebTest-ReportingName"];
                        if (!string.IsNullOrEmpty(sReportingName))
                        {
                            oXML.WriteAttributeString("ReportingName", sReportingName);
                        }
                        int ixQueryPart = oSession.url.IndexOf("?");
                        if (ixQueryPart <= -1)
                        {
                            sBaseURL = oSession.url;
                        }
                        else
                        {
                            sBaseURL = oSession.url.Substring(0, ixQueryPart);
                            string[] strArrays = oSession.url.Substring(ixQueryPart + 1).Split(new char[] { '&' });
                            string sNonNVPs = string.Empty;
                            string[] strArrays1 = strArrays;
                            for (timeout = 0; timeout < (int)strArrays1.Length; timeout++)
                            {
                                string sNVP = strArrays1[timeout];
                                if (sNVP.Length > 0 && !sNVP.Contains("="))
                                {
                                    sNonNVPs = (sNonNVPs.Length != 0 ? string.Concat(sNonNVPs, "&") : "?");
                                    sNonNVPs = string.Concat(sNonNVPs, sNVP);
                                }
                            }
                            sBaseURL = string.Concat(sBaseURL, sNonNVPs);
                        }
                        oXML.WriteAttributeString("Url", string.Format("{0}://{1}", oSession.oRequest.headers.UriScheme, sBaseURL));
                        oXML.WriteAttributeString("ThinkTime", thinkTime);
                        timeout = webTestSession.Timeout;
                        oXML.WriteAttributeString("Timeout", timeout.ToString());
                        preAuthenticate = webTestSession.ParseDependentRequests;
                        oXML.WriteAttributeString("ParseDependentRequests", preAuthenticate.ToString());
                        preAuthenticate = webTestSession.FollowRedirects;
                        oXML.WriteAttributeString("FollowRedirects", preAuthenticate.ToString());
                        preAuthenticate = webTestSession.RecordResults;
                        oXML.WriteAttributeString("RecordResult", preAuthenticate.ToString());
                        preAuthenticate = webTestSession.CacheControl;
                        oXML.WriteAttributeString("Cache", preAuthenticate.ToString());
                        timeout = webTestSession.ResponseTimeGoal;
                        oXML.WriteAttributeString("ResponseTimeGoal", timeout.ToString());
                        oXML.WriteAttributeString("Encoding", webTestSession.TextEncoding);
                        oXML.WriteStartElement("Headers");
                        foreach (Header oHeader in webTestSession.Headers)
                        {
                            if (!oHeader.WriteToWebTest)
                            {
                                continue;
                            }
                            oXML.WriteStartElement("Header");
                            if (oHeader.Name != null)
                            {
                                oXML.WriteAttributeString("Name", oHeader.Name);
                            }
                            else
                            {
                                oXML.WriteAttributeString("Name", string.Empty);
                            }
                            if (oHeader.Value != null)
                            {
                                oXML.WriteAttributeString("Value", oHeader.Value);
                            }
                            else
                            {
                                oXML.WriteAttributeString("Value", string.Empty);
                            }
                            oXML.WriteEndElement();
                        }
                        oXML.WriteEndElement();
                        if (webTestSession.ValidationRules.Count > 0)
                        {
                            oXML.WriteStartElement("ValidationRules");
                            foreach (ValidationRule rule in webTestSession.ValidationRules)
                            {
                                oXML.WriteStartElement("ValidationRule");
                                oXML.WriteAttributeString("Classname", rule.Classname);
                                oXML.WriteAttributeString("Level", rule.ValidationLevel.ToString());
                                if (rule.Properties.Count > 0)
                                {
                                    oXML.WriteStartElement("RuleParameters");
                                    foreach (RuleProperty rProperty in rule.Properties)
                                    {
                                        oXML.WriteStartElement("RuleParameter");
                                        oXML.WriteAttributeString("Name", rProperty.Name);
                                        oXML.WriteAttributeString("Value", rProperty.Value);
                                        oXML.WriteEndElement();
                                    }
                                    oXML.WriteEndElement();
                                }
                                oXML.WriteEndElement();
                            }
                            oXML.WriteEndElement();
                        }
                        if (webTestSession.ExtractionRules.Count > 0)
                        {
                            oXML.WriteStartElement("ExtractionRules");
                            foreach (ExtractionRule rule in webTestSession.ExtractionRules)
                            {
                                oXML.WriteStartElement("ExtractionRule");
                                oXML.WriteAttributeString("Classname", rule.Classname);
                                if (rule.ContextParameterName != null)
                                {
                                    oXML.WriteAttributeString("VariableName", rule.ContextParameterName);
                                }
                                if (rule.Properties.Count > 0)
                                {
                                    oXML.WriteStartElement("RuleParameters");
                                    foreach (RuleProperty rProperty in rule.Properties)
                                    {
                                        oXML.WriteStartElement("RuleParameter");
                                        oXML.WriteAttributeString("Name", rProperty.Name);
                                        oXML.WriteAttributeString("Value", rProperty.Value);
                                        oXML.WriteEndElement();
                                    }
                                    oXML.WriteEndElement();
                                }
                                oXML.WriteEndElement();
                            }
                            oXML.WriteEndElement();
                        }
                        if (webTestSession.RequestQueryParams.Count > 0)
                        {
                            oXML.WriteStartElement("QueryStringParameters");
                            foreach (QueryStringParameter oQSP in webTestSession.RequestQueryParams)
                            {
                                oXML.WriteStartElement("QueryStringParameter");
                                if (oQSP.Name != null)
                                {
                                    oXML.WriteAttributeString("Name", oQSP.Name);
                                }
                                else
                                {
                                    oXML.WriteAttributeString("Name", string.Empty);
                                }
                                if (oQSP.Value != null)
                                {
                                    oXML.WriteAttributeString("Value", oQSP.Value);
                                }
                                else
                                {
                                    oXML.WriteAttributeString("Value", string.Empty);
                                }
                                preAuthenticate = oQSP.UrlEncode;
                                oXML.WriteAttributeString("UrlEncode", preAuthenticate.ToString());
                                preAuthenticate = oQSP.UseToGroupResults;
                                oXML.WriteAttributeString("UseToGroupResults", preAuthenticate.ToString());
                                oXML.WriteEndElement();
                            }
                            oXML.WriteEndElement();
                        }
                        if (!Utilities.IsNullOrEmpty(oSession.requestBodyBytes))
                        {
                            string sContentType = oSession.oRequest["Content-Type"];
                            if (oSession.oRequest.headers.ExistsAndContains("Content-Type", "application/x-www-form-urlencoded"))
                            {
                                btBodyEncoding = FiddlerWebTest.BodyType.URLEncoded;
                                oXML.WriteStartElement("FormPostHttpBody");
                            }
                            else if (!Utilities.IsBinaryMIME(sContentType))
                            {
                                btBodyEncoding = FiddlerWebTest.BodyType.String;
                                oXML.WriteStartElement("StringHttpBody");
                            }
                            else
                            {
                                oXML.WriteStartElement("BinaryHttpBody");
                                btBodyEncoding = FiddlerWebTest.BodyType.Binary;
                            }
                            if (!string.IsNullOrEmpty(sContentType))
                            {
                                oXML.WriteAttributeString("ContentType", sContentType);
                            }
                            switch (btBodyEncoding)
                            {
                                case FiddlerWebTest.BodyType.URLEncoded:
                                    {
                                        using (IEnumerator<FormPostParameter> enumerator = webTestSession.RequestFormParams.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                FormPostParameter formParam = enumerator.Current;
                                                oXML.WriteStartElement("FormPostParameter");
                                                if (formParam.Name != null)
                                                {
                                                    oXML.WriteAttributeString("Name", formParam.Name);
                                                }
                                                else
                                                {
                                                    oXML.WriteAttributeString("Name", string.Empty);
                                                }
                                                if (formParam.Value != null)
                                                {
                                                    oXML.WriteAttributeString("Value", formParam.Value);
                                                }
                                                else
                                                {
                                                    oXML.WriteAttributeString("Value", string.Empty);
                                                }
                                                preAuthenticate = formParam.UrlEncode;
                                                oXML.WriteAttributeString("UrlEncode", preAuthenticate.ToString());
                                                oXML.WriteEndElement();
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                case FiddlerWebTest.BodyType.String:
                                    {
                                        oXML.WriteString(Convert.ToBase64String(Encoding.Unicode.GetBytes(oSession.GetRequestBodyAsString())));
                                        break;
                                    }
                                case FiddlerWebTest.BodyType.Binary:
                                    {
                                        oXML.WriteString(Convert.ToBase64String(oSession.requestBodyBytes));
                                        break;
                                    }
                                default:
                                    {
                                        throw new NotImplementedException("Impossible: unknown format");
                                    }
                            }
                            oXML.WriteEndElement();
                        }
                        oXML.WriteEndElement();
                        if (evtProgressNotifications != null)
                        {
                            timeout = i + 1;
                            evtProgressNotifications(null, new ProgressCallbackEventArgs((float)(i + 1) / (float)this.Sessions.Count, string.Concat("wrote ", timeout.ToString(), " sessions to WebTest.")));
                        }
                    }
                    else
                    {
                        if (evtProgressNotifications != null)
                        {
                            evtProgressNotifications(null, new ProgressCallbackEventArgs((float)(i + 1) / (float)this.Sessions.Count, string.Concat("Ignoring Session #", oSession.id, " for WebTest.")));
                        }
                        FiddlerWebTest._WriteSessionComment(oSession, oXML, false);
                    }
                }
                if (usingNex)
                {
                    oXML.WriteEndElement();
                    oXML.WriteEndElement();
                }
                oXML.WriteEndElement();
                oXML.WriteEndDocument();
                oXML.Close();
            }
            catch (Exception exception)
            {
                Exception eX = exception;
                MessageBox.Show(string.Concat("Failed to save test\n", eX.Message));
            }
        }

        public event EventHandler<PreWebTestSaveEventArgs> PreWebTestSave;

        private enum BodyType : byte
        {
            Unknown,
            URLEncoded,
            String,
            Binary
        }
    }
}