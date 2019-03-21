using System;

namespace Fiddler.WebTesting
{
	public class PreWebTestSaveEventArgs : EventArgs
	{
		private Fiddler.WebTesting.FiddlerWebTest m_webTest;

		public Fiddler.WebTesting.FiddlerWebTest FiddlerWebTest
		{
			get
			{
				return this.m_webTest;
			}
		}

		internal PreWebTestSaveEventArgs(Fiddler.WebTesting.FiddlerWebTest WebTest)
		{
			this.m_webTest = WebTest;
		}
	}
}