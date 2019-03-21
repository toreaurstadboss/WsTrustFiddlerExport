using System;

namespace Fiddler.WebTesting
{
	public interface IFiddlerWebTestPlugin
	{
		void PreWebTestSave(object sender, PreWebTestSaveEventArgs e);
	}
}