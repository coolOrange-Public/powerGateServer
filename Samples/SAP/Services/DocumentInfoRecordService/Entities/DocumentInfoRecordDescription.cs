using System.Data.Services.Common;

namespace UserServices.Entities
{
    [DataServiceKey("Documentversion", "Documenttype", "Documentpart", "Documentnumber", "Langu")]
    [DataServiceEntity]
    public class DocumentInfoRecordDescription
    {
        public string Documentversion { get; set; }
        public string Documenttype { get; set; }
        public string Documentpart { get; set; }
        public string Documentnumber { get; set; }

        public string Langu { get; set; }
        public string Description { get; set; }
    }
}