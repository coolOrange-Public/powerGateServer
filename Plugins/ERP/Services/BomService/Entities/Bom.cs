using System.Collections.Generic;
using System.Data.Services.Common;

namespace ErpServices.Services.BomService.Entities
{
	[DataServiceKey("ParentNumber")]
	[DataServiceEntity]
	public class Bom
	{
		public string ParentNumber { get; set; }
		public IEnumerable<BomItem> Children { get; set; }

		public Bom()
		{
			Children = new List<BomItem>();
		}
	}
}