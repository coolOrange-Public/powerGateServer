using System.Data.Services.Common;

namespace ErpServices.Services.MaterialService.Entities
{
	[DataServiceKey("Number", "Language")]
	[DataServiceEntity]
	public class MaterialDescription
	{
		public string Number { get; set; }
		public string Language { get; set; }
		public string Description { get; set; }
	}
}