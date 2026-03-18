namespace ErpServices.Converters
{
	public interface ITypeConverter<TFrom, TTo>
	{
		TTo ConvertTo(TFrom from);
		TFrom ConvertFrom(TTo to);
	}
}