using Fiddler;
using System;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class FilterRedirects : IFiddlerWebTestPlugin
	{
		public FilterRedirects()
		{
		}

		private static bool IsRedirectedRequest(Session oSession)
		{
			return Utilities.IsRedirectStatus(oSession.responseCode);
		}

		public void PreWebTestSave(object sender, PreWebTestSaveEventArgs e)
		{
			bool isPreviousResponseRedirect = false;
			for (int i = 0; i < e.FiddlerWebTest.Sessions.Count; i++)
			{
				WebTestSession session = e.FiddlerWebTest.Sessions[i];
				if (session.WriteToWebTest)
				{
					session.FollowRedirects = true;
					if (isPreviousResponseRedirect)
					{
						WebTestSession previousSession = e.FiddlerWebTest.Sessions[i - 1];
						if (previousSession != null && previousSession.FiddlerSession != null && previousSession.FiddlerSession.bHasResponse)
						{
							string sRedirTarget = previousSession.FiddlerSession.GetRedirectTargetURL();
							if (sRedirTarget != null)
							{
								sRedirTarget = Utilities.TrimAfter(sRedirTarget, '#');
								if (sRedirTarget.Equals(session.FiddlerSession.fullUrl))
								{
									session.ParentSession = previousSession;
									previousSession.ChildSession = session;
									session.WriteToWebTest = false;
								}
							}
						}
					}
					isPreviousResponseRedirect = FilterRedirects.IsRedirectedRequest(session.FiddlerSession);
				}
			}
		}
	}
}