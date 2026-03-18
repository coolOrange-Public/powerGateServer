using System.Collections.Generic;
using System.Data.Services.Common;

namespace SapServices.Services.BillofMaterialService.Entities
{
    [DataServiceKey("Plant", "BOMUsage", "Material", "Alternative")]
    [DataServiceEntity]
    public class BillOfMaterialContext
    {
        public string Plant { get; set; }
        public string BOMUsage { get; set; }
        public string Material { get; set; }
        public string Alternative { get; set; }
        public IEnumerable<BillOfMaterialDocumentAssign> BOMDocAssign { get; set; }
        public IEnumerable<BillOfMaterialItemData> BOMItemData { get; set; }
        public BillOfMaterialHeaderData BillOfMaterialHeaderData { get; set; }

        public BillOfMaterialContext()
        {
            BOMDocAssign = new List<BillOfMaterialDocumentAssign>();
            BOMItemData = new List<BillOfMaterialItemData>();
        }
    }
}