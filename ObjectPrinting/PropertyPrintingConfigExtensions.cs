using System.Globalization;

namespace ObjectPrinting
{
	public static class PropertyPrintingConfigExtensions
	{
		public static PrintingConfig<TOwner> UsingCulture<TOwner>(
			this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
			CultureInfo cultureInfo)
		{
			var printingConfig = ((IPropertyPrintingConfig<TOwner>)propertyPrintingConfig)
				.PrintingConfig;
			printingConfig.Cultures[typeof(double)] = cultureInfo;
			return printingConfig;
		}

		public static PrintingConfig<TOwner> UsingCulture<TOwner>(
			this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig,
			CultureInfo cultureInfo)
		{
			var printingConfig = ((IPropertyPrintingConfig<TOwner>)propertyPrintingConfig)
				.PrintingConfig;
			printingConfig.Cultures[typeof(int)] = cultureInfo;
			return printingConfig;
		}

		public static PrintingConfig<TOwner> Trim<TOwner>(
			this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
			int maxLength)
		{
			var printingConfig = ((IPropertyPrintingConfig<TOwner>)propertyPrintingConfig)
				.PrintingConfig;
			printingConfig.MaxLength = maxLength;
			return printingConfig;
		}
	}
}