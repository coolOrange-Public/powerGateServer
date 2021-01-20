using ErpServices.Services.MaterialService.Entities;
using powerGateServer.SDK;

namespace ErpServices.Services.MaterialService
{
	[WebServiceData("PGS/ERP", "MaterialService")]
	public class MaterialService : ErpServiceBase
	{
		protected override void CreateTablesForEntities()
		{
			EntityStores.AddStoreFor<MaterialDescription>();
			EntityStores.AddStoreFor<Material>();
		}

		protected override void RegisterLookups()
		{
			AddLookupFor<MaterialTypeLookup>();
			AddLookupFor<UomLookup>();
			AddLookupFor<LanguageLookup>();
		}

		protected override void RegisterEntitySets()
		{
			AddEntitySetOfType<Materials>();
			AddEntitySetOfType<MaterialDescriptions>();
		}
	}
}
