using powerGateServer.SDK;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	[WebServiceData("sap/opu/odata/Arcona6", "MATERIAL_SRV")]
	public class MaterialService : SapServiceBase
	{
		protected override void CreateTablesForEntities()
		{
			EntityStores.AddStoreFor<Description>();
			EntityStores.AddStoreFor<PlantData>();
			EntityStores.AddStoreFor<ValuationData>();
			EntityStores.AddStoreFor<BasicData>();
			EntityStores.AddStoreFor<MaterialContext>();
		}

		protected override void RegisterLookups()
		{
			AddLookupFor<MatlTypeLookup>();
			AddLookupFor<MatlGroupLookup>();
			AddLookupFor<PurGroupLookup>();
			AddLookupFor<PurStatusLookup>();
			AddLookupFor<IndSectorLookup>();
			AddLookupFor<AvailcheckLookup>();
			AddLookupFor<LanguLookup>();
			AddLookupFor<BaseUomIsoLookup>();
			AddLookupFor<BaseUomLookup>();
			AddLookupFor<LanguIsoHelpValue>();
			AddLookupFor<ValuationTypeAndCategoryLookup>();
			AddLookupFor<ValuationTypeLookup>();
			AddLookupFor<PriceControlLookup>();
			AddLookupFor<PlantLookup>();
			AddLookupFor<ValuationAreaLookup>();
			AddLookupFor<MaterialByPlantLookupCollection, MaterialByPlantLookup>();
			AddLookupFor<LanguForLanguIsoCollection, LanguForLanguIsoLookup>();
		}

		protected override void RegisterEntitySets()
		{
			AddEntitySetOfType<MaterialContextCollection>();
			AddEntitySetOfType<DescriptionCollection>();
			AddEntitySetOfType<PlantDataCollection>();
			AddEntitySetOfType<BasicDataCollection>();
			AddEntitySetOfType<ValuationDataCollection>();
		}
	}
}
