using UserServices.Services.Entities;

namespace UserServices.Services.BillofMaterialService
{
	public class BillOfMaterialService : SapServiceBase
	{
		public override string Name
		{
			get { return "Arcona6/BILL_OF_MATERIAL_SRV"; }
		}

		protected override void CreateTablesForEntities()
		{
            EntityStores.AddStoreFor<BillOfMaterialHeaderData>();
            EntityStores.AddStoreFor<BillOfMaterialItemData>();
            EntityStores.AddStoreFor<BillOfMaterialDocumentAssign>();
            EntityStores.AddStoreFor<BillOfMaterialContext>();
		}

		protected override void RegisterEntitySets()
		{
            AddEntitySetOfType<BOMHeaderDataCollection>();
            AddEntitySetOfType<BOMItemDataCollection>();
            AddEntitySetOfType<BOMDocumentAssignCollection>();
            AddEntitySetOfType<BillOfMaterialContextCollection>();
		}

		protected override void RegisterLookups()
		{
            AddLookupFor<PlantLookup>();
            AddLookupFor<BaseUnitOfMeasureLookup>();
            AddLookupFor<BOMGroupLookup>();
            AddLookupFor<DIRTextsLookup>();
            AddLookupFor<MaterialLookup>();
            AddLookupFor<BOMUsageLookup>();
            AddLookupFor<MaterialGroupLookup>();
            AddLookupFor<BOMForMaterialLookup>();
            AddLookupFor<ItemCatLookup>();
		}
	}
}
