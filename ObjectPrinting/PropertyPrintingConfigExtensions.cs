using System.Globalization;

namespace ObjectPrinting
{
	public static class PropertyPrintingConfigExtensions
	{
		public static PrintingConfig<TOwner> UsingCulture<TOwner>(
			this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig,
			CultureInfo cultureInfo)
		{
			return ((IPropertyPrintingConfig<TOwner>)propertyPrintingConfig).PrintingConfig;
		}

		public static PrintingConfig<TOwner> Trim<TOwner>(
			this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig,
			int maxLength)
		{
			return ((IPropertyPrintingConfig<TOwner>)propertyPrintingConfig).PrintingConfig;
		}
	}
}