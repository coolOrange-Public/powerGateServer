using System.Collections.Generic;
using System.Data.Services.Common;

namespace ErpServices.Services.MaterialService.Entities
{
	[DataServiceKey("Number")]
	[DataServiceEntity]
	public class Material
	{
		public string Number { get; set; }
		public string UnitOfMeasure { get; set; }
		public string Type { get; set; }
		public IEnumerable<MaterialDescription> Descriptions { get; set; }

		public Material()
		{
			Descriptions = new List<MaterialDescription>();
		}
	}
}
