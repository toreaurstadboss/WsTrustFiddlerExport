using System;

namespace Fiddler.WebTesting
{
	public class CorrelateViewState : WebTestPluginDynamicField
	{
		private const string ViewState = "__VIEWSTATE";

		public CorrelateViewState()
		{
			base.Field = "__VIEWSTATE";
		}
	}
}