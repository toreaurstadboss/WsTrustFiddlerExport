using System;

namespace Fiddler.WebTesting
{
	public class CorrelateEventValidation : WebTestPluginDynamicField
	{
		private const string EventValidation = "__EVENTVALIDATION";

		public CorrelateEventValidation()
		{
			base.Field = "__EVENTVALIDATION";
		}
	}
}