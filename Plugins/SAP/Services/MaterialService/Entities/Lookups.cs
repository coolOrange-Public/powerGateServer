using System.Data.Services.Common;

namespace SapServices.Services.MaterialService.Entities
{
	[DataServiceKey("MatlType")]
	public class MatlTypeLookup
	{
		public string MatlType { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("MatlGroup")]
	public class MatlGroupLookup
	{
		public string MatlGroup { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("PurGroup")]
	public class PurGroupLookup
	{
		public string PurGroup { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("PurStatus")]
	public class PurStatusLookup
	{
		public string PurStatus { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("IndSector")]
	public class IndSectorLookup
	{
		public string IndSector { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("Availcheck")]
	public class AvailcheckLookup
	{
		public string Availcheck { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("Langu")]
	public class LanguLookup
	{
		public string Langu { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("BaseUomIso")]
	public class BaseUomIsoLookup
	{
		public string BaseUomIso { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("BaseUom")]
	public class BaseUomLookup
	{
		public string BaseUom { get; set; }
		public string Description { get; set; }
	}


	[DataServiceKey("LanguIso", "Langu")]
	public class LanguIsoHelpValue
	{
		public string LanguIso { get; set; }
		public string Langu { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("LanguIso", "Langu")]
	public class LanguForLanguIsoLookup
	{
		public string LanguIso { get; set; }
		public string Langu { get; set; }
	}

	[DataServiceKey("ValType", "ValCategory", "ValArea")]
	public class ValuationTypeAndCategoryLookup
	{
		public string ValType { get; set; }
		public string ValCategory { get; set; }
		public string ValArea { get; set; }
	}

	[DataServiceKey("ValuationType")]
	public class ValuationTypeLookup
	{
		public string ValuationType { get; set; }
		public string ValuationArea { get; set; }
	}

	[DataServiceKey("PriceControl")]
	public class PriceControlLookup
	{
		public string PriceControl { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("Plant")]
	public class PlantLookup
	{
		public string Plant { get; set; }
		public string Description { get; set; }
	}

	[DataServiceKey("ValArea", "Plant", "MatlType")]
	public class ValuationAreaLookup
	{
		public string Plant { get; set; }
        public string ValArea { get; set; }
		public string MatlType { get; set; }
	}

	[DataServiceKey("Material", "Plant", "Langu")]
	public class MaterialByPlantLookup
	{
		public string Material { get; set; }
		public string Plant { get; set; }
		public string Langu { get; set; }
		public string Description { get; set; }
	}
}
