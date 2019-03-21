using System;

namespace Fiddler.WebTesting
{
	public class ExtractionRule : Rule
	{
		public const string ExtractHiddenFieldsClassName = "Microsoft.VisualStudio.TestTools.WebTesting.Rules.ExtractHiddenFields, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		public const string ExtractTextClassName = "Microsoft.VisualStudio.TestTools.WebTesting.Rules.ExtractText, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		public const string ExtractFormFieldsClassName = "Microsoft.VisualStudio.TestTools.WebTesting.Rules.ExtractFormField, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		public const string VariableName = "VariableName";

		public const string StartsWith = "StartsWith";

		public const string EndsWith = "EndsWith";

		public const string IgnoreCase = "IgnoreCase";

		public const string UseRegularExpression = "UseRegularExpression";

		public const string Required = "Required";

		public const string Index = "Index";

		public const string Name = "Name";

		public string m_ContextParameterName;

		public string ContextParameterName
		{
			get
			{
				return this.m_ContextParameterName;
			}
			set
			{
				this.m_ContextParameterName = value;
			}
		}

		public ExtractionRule(string Classname) : base(Classname)
		{
		}
	}
}