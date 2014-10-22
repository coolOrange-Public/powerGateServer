using UserServices.Entities;
using UserServices.Services;
using UserServices.Services.MaterialService;

namespace UserServices.ServiceDefinition
{
	public class MaterialService : SapServiceBase
	{
		public override string Name
		{
			get { return "Arcona6/MATERIAL_SRV"; }
		}

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
