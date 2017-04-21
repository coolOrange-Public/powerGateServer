using System.Data.Services.Common;

namespace SapServices.Services.BillofMaterialService.Entities
{
    [DataServiceKey("ItemGUID", "Plant", "BOMUsage", "Material", "Alternative")]
    [DataServiceEntity]
    public class BillOfMaterialItemData
    {
        public string ItemGUID { get; set; }
        public string Plant { get; set; }
        public string BOMUsage { get; set; }
        public string Material { get; set; }
        public string Alternative { get; set; }

        public string ItemCat { get; set; }
        public string ItemNo { get; set; }
        public string Component { get; set; }
        public string ComponentQty { get; set; }
        public string ComponentUnit { get; set; }
        public string RecursionAllowed { get; set; }
        public string ItemID { get; set; }
        public string MaterialGroup { get; set; }
    }
}