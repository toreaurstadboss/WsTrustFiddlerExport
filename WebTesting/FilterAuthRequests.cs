using Fiddler;
using System;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class FilterAuthRequests : IFiddlerWebTestPlugin
	{
		public FilterAuthRequests()
		{
		}

		public void PreWebTestSave(object sender, PreWebTestSaveEventArgs e)
		{
			for (int i = 0; i < e.FiddlerWebTest.Sessions.Count; i++)
			{
				WebTestSession session = e.FiddlerWebTest.Sessions[i];
				if (session.FiddlerSession.oResponse != null && session.FiddlerSession.oResponse.headers != null && (session.FiddlerSession.oResponse.headers.HTTPResponseCode == 401 || session.FiddlerSession.oResponse.headers.HTTPResponseCode == 407))
				{
					session.WriteToWebTest = false;
				}
			}
		}
	}
}