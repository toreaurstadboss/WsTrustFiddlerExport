using System;

namespace WsTrustFiddlerWebTestExport.WebTesting
{
	public class PreWebTestSaveEventArgs : EventArgs
	{
		private FiddlerWebTest m_webTest;

		public FiddlerWebTest FiddlerWebTest
		{
			get
			{
				return this.m_webTest;
			}
		}

		internal PreWebTestSaveEventArgs(FiddlerWebTest WebTest)
		{
			this.m_webTest = WebTest;
		}
	}
}