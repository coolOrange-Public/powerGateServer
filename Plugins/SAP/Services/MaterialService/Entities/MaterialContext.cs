using System.Collections.Generic;
using System.Data.Services.Common;

namespace SapServices.Services.MaterialService.Entities
{
	[DataServiceKey("Material", "Plant", "ValuationArea", "ValuationType")]
	[DataServiceEntity]
	public class MaterialContext
	{
		public string Material { get; set; }
		public string Plant { get; set; }
		public string ValuationArea { get; set; }
		public string ValuationType { get; set; }
		public PlantData PlantData { get; set; }
		public ValuationData ValuationData { get; set; }
		public IEnumerable<Description> Description { get; set; }
		public BasicData BasicData { get; set; }

		public MaterialContext()
		{
			Description = new List<Description>();
		}
	}
}
