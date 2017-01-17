using System.Data.Services.Common;

namespace SapServices.Services.BillofMaterialService.Entities
{
    [DataServiceKey("Plant", "BOMUsage", "Material", "Alternative")]
    [DataServiceEntity]
    public class BillOfMaterialHeaderData
    {
        public string Plant { get; set; }
        public string BOMUsage { get; set; }
        public string Material { get; set; }
        public string Alternative { get; set; }

        public string BaseQuan { get; set; }
        public string BaseUnit { get; set; }
        public string BOMStatus { get; set; }
        public string AltText { get; set; }
        public string Laboratory { get; set; }
        public string BOMText { get; set; }
        public string BOMGroup { get; set; }
        public string AuthGroup { get; set; }
        public string CadInd { get; set; }
        public string HeaderGUID { get; set; }
    }
}