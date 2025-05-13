using System.Data.Services.Common;

namespace SapServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documentversion", "Documenttype", "Documentpart", "Documentnumber", "ClassType", "ClassName")]
	[DataServiceEntity]
	public class Classification
	{
		public string Documentversion { get; set; }
		public string Documenttype { get; set; }
		public string Documentpart { get; set; }
		public string Documentnumber { get; set; }

		public string ClassType { get; set; }
		public string ClassName { get; set; }
		public string Status { get; set; }
		public string StandardClassIndicator { get; set; }
		public string DeleteAllocation { get; set; }
	}
}