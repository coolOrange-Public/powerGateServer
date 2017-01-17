using System.Data.Services.Common;

namespace ErpServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documenttype", "Documentnumber", "Documentversion", "Documentpart")]
	[DataServiceEntity]
	public class DocumentInfoRecordData
	{
		public string Documenttype { get; set; }
		public string Documentnumber { get; set; }
		public string Documentversion { get; set; }
		public string Documentpart { get; set; }
		public string Description { get; set; }
		public string StatusIntern { get; set; }
	}
}