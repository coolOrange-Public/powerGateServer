using System.Collections.Generic;
using UserServices.Entities;
using UserServices.ServiceDefinition;
using UserServices.Services.Entities;

namespace UserServices.Services
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