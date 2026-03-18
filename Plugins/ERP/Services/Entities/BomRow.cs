using System.Data.Services.Common;

namespace ErpServices.Services.Entities
{
	[DataServiceKey("ParentNumber", "ChildNumber")]
	[DataServiceEntity]
	public class BomRow
	{
		 public string ParentNumber { get; set; }
		 public string ChildNumber { get; set; }
		 public int Position { get; set; }
		 public decimal Quantity { get; set; }
		 public Item Item { get; set; }
		 
		 public BomRow(){
		 }
		 public BomRow(BomRow bomRow)
		 {
			 ParentNumber = bomRow.ParentNumber;
			 ChildNumber = bomRow.ChildNumber;
			 Position = bomRow.Position;
			 Quantity = bomRow.Quantity;
			 Item = bomRow.Item;
		 }
	}
}