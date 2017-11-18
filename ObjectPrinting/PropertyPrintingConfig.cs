using System;
using System.Reflection;

namespace ObjectPrinting
{
	public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner>
	{
		private readonly PrintingConfig<TOwner> printingConfig;
		private readonly PropertyInfo property;

		public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
			: this(printingConfig, null)
		{ }

		public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, PropertyInfo property)
		{
			this.printingConfig = printingConfig;
			this.property = property;
		}

		public PrintingConfig<TOwner> Using(Func<TPropType, string> serializer)
		{
			return printingConfig;
		}

		PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner>.PrintingConfig => printingConfig;
	}
}