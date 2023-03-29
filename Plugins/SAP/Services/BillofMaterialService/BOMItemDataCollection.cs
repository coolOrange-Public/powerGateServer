using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
    public class BOMItemDataCollection : BomNavigationPropertyCollectionEntitySet<BillOfMaterialItemData>
    {
        public BOMItemDataCollection(IEntityStores entityStores) : base(entityStores)
        {
        }

        public override string Name
        {
            get { return "BOMItemDataCollection"; }
        }
    }
}