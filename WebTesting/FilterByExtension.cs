using Fiddler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fiddler.WebTesting
{
	public class FilterByExtension : IFiddlerWebTestPlugin
	{
		private string m_extensionsToExclude = ".js;.vbscript;.gif;.jpg;.jpeg;.jpe;.png;.css;.rss";

		public string ExtensionsToExclude
		{
			get
			{
				return this.m_extensionsToExclude;
			}
			set
			{
				this.m_extensionsToExclude = value;
			}
		}

		public FilterByExtension()
		{
		}

		private void FilterRequestsByExtension(List<string> extensionsToExclude, WebTestSession session)
		{
			string path = session.FiddlerSession.PathAndQuery;
			int idxOfQueryString = path.IndexOf('?');
			if (idxOfQueryString > -1)
			{
				path = path.Substring(0, idxOfQueryString);
			}
			int idxOfExtension = path.LastIndexOf('.');
			if (idxOfExtension > -1 && extensionsToExclude.Contains(path.Substring(idxOfExtension)))
			{
				session.WriteToWebTest = false;
			}
		}

		private List<string> ParseExtensions()
		{
			List<string> extenstions = new List<string>();
			if (this.ExtensionsToExclude != null && this.ExtensionsToExclude.Length > 0)
			{
				string[] strArrays = new string[] { ";" };
				string[] strArrays1 = this.ExtensionsToExclude.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays1.Length; i++)
				{
					extenstions.Add(strArrays1[i]);
				}
			}
			return extenstions;
		}

		public void PreWebTestSave(object sender, PreWebTestSaveEventArgs e)
		{
			List<string> extensionsToExclude = this.ParseExtensions();
			for (int i = 0; i < e.FiddlerWebTest.Sessions.Count; i++)
			{
				this.FilterRequestsByExtension(extensionsToExclude, e.FiddlerWebTest.Sessions[i]);
			}
		}
	}
}