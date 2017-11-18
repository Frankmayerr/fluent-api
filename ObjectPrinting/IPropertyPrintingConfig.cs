namespace ObjectPrinting
{
	interface IPropertyPrintingConfig<TOwner>
	{
		PrintingConfig<TOwner> PrintingConfig { get; }
	}
}