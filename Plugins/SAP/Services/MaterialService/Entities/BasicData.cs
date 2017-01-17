using System.Data.Services.Common;

namespace SapServices.Services.MaterialService.Entities
{
	[DataServiceKey("Material")]
	public class BasicData
	{
		public string Material { get; set; }
		public string BaseUomIso { get; set; }
		public string BaseUom { get; set; }
		public string MatlGroup { get; set; }
		public string MatlType { get; set; }
		public string IndSector { get; set; }
	}
}