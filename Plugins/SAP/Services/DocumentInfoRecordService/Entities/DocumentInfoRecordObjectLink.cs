using System.Data.Services.Common;

namespace SapServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documentversion", "Documenttype", "Documentpart", "Documentnumber", "ObjectType", "ObjectKey")]
	[DataServiceEntity]
	public class DocumentInfoRecordObjectLink
	{
		public string Documentversion { get; set; }
		public string Documenttype { get; set; }
		public string Documentpart { get; set; }
		public string Documentnumber { get; set; }
		public string ObjectType { get; set; }
		public string ObjectKey { get; set; }
	}
}