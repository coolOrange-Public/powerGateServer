using UserServices.Services.Entities;

namespace UserServices.Services
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