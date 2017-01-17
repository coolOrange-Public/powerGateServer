using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
    public class BOMHeaderDataCollection : SapServiceMethodBase<BillOfMaterialHeaderData>
    {
        public override string Name
        {
            get { return "BOMHeaderDataCollection"; }
        }

        public BOMHeaderDataCollection(IEntityStores entityStores)
            : base(entityStores)
        {
        }
    }
}