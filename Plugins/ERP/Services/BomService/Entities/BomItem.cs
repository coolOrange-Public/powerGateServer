using System.Data.Services.Common;

namespace ErpServices.Services.BomService.Entities
{
	[DataServiceKey("ParentNumber", "ChildNumber", "Position")]
	[DataServiceEntity]
	public class BomItem
	{
		 public string ParentNumber { get; set; }
		 public string ChildNumber { get; set; }
		 public int Position { get; set; }
		 public int Quantity { get; set; }
	}
}