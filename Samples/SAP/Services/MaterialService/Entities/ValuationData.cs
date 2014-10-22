using System.Data.Services.Common;

namespace UserServices.Entities
{
	[DataServiceKey("Material", "ValType", "ValArea")]
	public class ValuationData
	{
		public string Material { get; set; }
		public decimal PriceUnit { get; set; }
		public decimal StdPrice { get; set; }
		public string ValType { get; set; }
		public string ValArea { get; set; }
		public string PriceControl { get; set; }
		public string ValCategory { get; set; }
	}
}