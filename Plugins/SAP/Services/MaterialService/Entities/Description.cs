using System.Data.Services.Common;

namespace SapServices.Services.MaterialService.Entities
{
	[DataServiceKey("Material", "Langu")]
	[DataServiceEntity]
	public class Description
	{
		public string Material { get; set; }
		public string Langu { get; set; }
		public string LanguIso { get; set; }
		public string MatlDesc { get; set; }
	}
}