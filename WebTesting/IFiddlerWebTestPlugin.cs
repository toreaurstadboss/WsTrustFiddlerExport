namespace WsTrustFiddlerWebTestExport.WebTesting
{
	public interface IFiddlerWebTestPlugin
	{
		void PreWebTestSave(object sender, PreWebTestSaveEventArgs e);
	}
}