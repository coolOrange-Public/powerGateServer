using ErpServices.Services.Boms;
using ErpServices.Services.Entities;
using powerGateServer.SDK;

namespace ErpServices.Services
{
	[WebServiceData("PGS/ERP", "DemoService")]
	public class DemoService : ErpServiceBase
	{
		protected override void CreateTablesForEntities()
		{
			EntityStores.AddStoreFor<Item>();
			EntityStores.AddStoreFor<BomRow>();
			EntityStores.AddStoreFor<BomHeader>();
		}

		protected override void RegisterEntitySets()
		{
			AddEntitySetOfType<Items.Items>();
			AddEntitySetOfType<BomRows>();
			AddEntitySetOfType<BomHeaders>();
		}
	}
}