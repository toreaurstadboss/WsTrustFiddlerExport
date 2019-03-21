using Fiddler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class FilterByMimeType : IFiddlerWebTestPlugin
	{
		private string m_mimeTypesToExclude = "image;application/x-javascript;application/x-ns-proxy-autoconfig;text/css";

		public string MimeTypesToExclude
		{
			get
			{
				return this.m_mimeTypesToExclude;
			}
			set
			{
				this.m_mimeTypesToExclude = value;
			}
		}

		public FilterByMimeType()
		{
		}

		private void FilterRequestByMimeType(List<string> mimeTypesToExclude, WebTestSession session)
		{
			try
			{
				string contentType = session.FiddlerSession.oResponse["Content-Type"];
				if (!mimeTypesToExclude.Contains(contentType))
				{
					foreach (string mimeType in mimeTypesToExclude)
					{
						if (!contentType.StartsWith(mimeType))
						{
							continue;
						}
						session.WriteToWebTest = false;
					}
				}
				else
				{
					session.WriteToWebTest = false;
				}
			}
			catch (Exception exception)
			{
			}
		}

		private List<string> ParseMimeTypes()
		{
			List<string> mimeTypes = new List<string>();
			if (this.MimeTypesToExclude != null && this.MimeTypesToExclude.Length > 0)
			{
				string[] strArrays = new string[] { ";" };
				string[] strArrays1 = this.MimeTypesToExclude.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays1.Length; i++)
				{
					mimeTypes.Add(strArrays1[i]);
				}
			}
			return mimeTypes;
		}

		public void PreWebTestSave(object sender, PreWebTestSaveEventArgs e)
		{
			List<string> mimeTypesToExclude = this.ParseMimeTypes();
			for (int i = 0; i < e.FiddlerWebTest.Sessions.Count; i++)
			{
				this.FilterRequestByMimeType(mimeTypesToExclude, e.FiddlerWebTest.Sessions[i]);
			}
		}
	}
}