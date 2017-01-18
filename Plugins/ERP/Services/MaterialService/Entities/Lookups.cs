using System.Data.Services.Common;

namespace ErpServices.Services.MaterialService.Entities
{
	[DataServiceKey("Type")]
	public class MaterialTypeLookup
	{
		public string Type { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("UnitOfMeasure")]
	public class UomLookup
	{
		public string UnitOfMeasure { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("Language")]
	public class LanguageLookup
	{
		public string Language { get; set; }
		public string Description { get; set; }
	}
}
