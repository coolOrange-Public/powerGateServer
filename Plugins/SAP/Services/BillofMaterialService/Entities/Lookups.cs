using System.Data.Services.Common;

namespace SapServices.Services.BillofMaterialService.Entities
{
    [DataServiceKey("Plant")]
    public class PlantLookup
    {
        public string Plant { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("Material")]
    public class BaseUnitOfMeasureLookup
    {
        public string Material { get; set; }
        public string BaseUom { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("BOMGroup")]
    public class BOMGroupLookup
    {
        public string BOMGroup { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("DocType", "DocNumber", "DocVersion", "DocPart", "Langu")]
    public class DIRTextsLookup
    {
        public string DocType { get; set; }
        public string DocNumber { get; set; }
        public string DocVersion { get; set; }
        public string DocPart { get; set; }
        public string Langu { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("Material", "Plant", "Langu")]
    public class MaterialLookup
    {
        public string Material { get; set; }
        public string Plant { get; set; }
        public string Langu { get; set; }
        public string Description { get; set; }
        //public BaseUnitOfMeasureLookup BaseUom { get; set; }
    }

    [DataServiceKey("BOMUsage")]
    public class BOMUsageLookup
    {
        public string BOMUsage { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("MaterialGroup")]
    public class MaterialGroupLookup
    {
        public string MaterialGroup { get; set; }
        public string Description { get; set; }
    }

    [DataServiceKey("Material", "Plant", "BOMUsage")]
    public class BOMForMaterialLookup
    {
        public string Material { get; set; }
        public string Plant { get; set; }
        public string BOMUsage { get; set; }
        public string LotSize_FR { get; set; }
        public string LotSize_TO { get; set; }
        public string BOMNo { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedOn { get; set; }
        public string ChangedBy { get; set; }
        public string Alternative { get; set; }
    }

    [DataServiceKey("ItemCat")]
    public class ItemCatLookup
    {
        public string ItemCat { get; set; }
        public string Description { get; set; }
    }
}
