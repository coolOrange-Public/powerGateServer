using System.Data.Services.Common;

namespace SapServices.Services.BillofMaterialService.Entities
{
    [DataServiceKey("DocType", "DocNumber", "DocVersion", "DocPart")]
    [DataServiceEntity]
    public class BillOfMaterialDocumentAssign
    {
        public string DocType { get; set; }
        public string DocNumber { get; set; }
        public string DocVersion { get; set; }
        public string DocPart { get; set; }

        public string LinkedSAPObj { get; set; }
        public string CounterKey { get; set; }
        public string DocMgmtObjKey { get; set; }
        public string FlagDelete { get; set; }
    }
}