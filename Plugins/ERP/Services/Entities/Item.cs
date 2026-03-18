using System.Data.Services.Common;

namespace ErpServices.Services.Entities
{
	[DataServiceKey("Number")]
	[DataServiceEntity]
	public class Item
	{
		public string Number { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string UnitOfMeasure { get; set; }
		public decimal Weight { get; set; }
		public string Material { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public bool MakeBuy { get; set; }
		public string Supplier { get; set; }

		public Item()
		{
		}
	}
}
