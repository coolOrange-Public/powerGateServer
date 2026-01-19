using System.Data.Services.Common;

namespace SapServices.Services.MaterialService.Entities
{
	[DataServiceKey("Material", "Plant")]
	public class PlantData
	{
		public string Material { get; set; }
		public string Plant { get; set; }
		public string PurGroup { get; set; }
		public string Availcheck { get; set; }
		public string PurStatus { get; set; }
        public string Pvallidfrom { get; set; }
	}
}