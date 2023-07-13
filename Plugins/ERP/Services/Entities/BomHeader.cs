using System.Collections.Generic;
using System.Data.Services.Common;

namespace ErpServices.Services.Entities
{
	[DataServiceKey("Number")]
	[DataServiceEntity]
	public class BomHeader
	{
		public string Number { get; set; }
		public string Description { get; set; }
		public decimal BaseQuantity { get; set; }
		public IEnumerable<BomRow> Children { get; set; }

		public BomHeader()
		{
			Children = new List<BomRow>();
		}

		public BomHeader(BomHeader bomHeader)
		{
			Number = bomHeader.Number;
			Description = bomHeader.Description;
			BaseQuantity = bomHeader.BaseQuantity;
			Children = new List<BomRow>();
		}
	}
}