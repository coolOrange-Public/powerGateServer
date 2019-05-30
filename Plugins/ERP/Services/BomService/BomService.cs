using ErpServices.Services.BomService.Entities;
using powerGateServer.SDK;

namespace ErpServices.Services.BomService
{
	[WebServiceData("PGS/ERP", "BomService")]
	public class BomService : ErpServiceBase
	{

		protected override void CreateTablesForEntities()
		{
			EntityStores.AddStoreFor<BomItem>();
			EntityStores.AddStoreFor<Bom>();
		}

		protected override void RegisterEntitySets()
		{
			AddEntitySetOfType<BomItems>();
			AddEntitySetOfType<Boms>();
		}

		protected override void RegisterLookups()
		{
		}
	}
}
