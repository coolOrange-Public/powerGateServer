using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
	public class BillOfMaterialContextCollection : ContextEntitySetBase<BillOfMaterialContext, BillOfMaterialContext>
    {
        public BillOfMaterialContextCollection(IEntityStores entityStores)
            : base(entityStores)
        {
        }

        public override string Name
        {
            get { return "BOMContextCollection"; }
        }

        public override void Update(BillOfMaterialContext entity)
        {
            Throw(entity, "{0} has no property that can be updated because it has only keys! " +
                "For updating navigation properties please use the correct EntitySet!");
        }
    }
}