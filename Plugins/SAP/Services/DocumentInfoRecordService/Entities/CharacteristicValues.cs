using System.Data.Services.Common;

namespace SapServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documentversion", "Documenttype", "Documentpart", "Documentnumber", "ClassType", "CharName", "CharValue")]
	[DataServiceEntity]
	public class CharacteristicValues
	{
		public string Documentversion { get; set; }
		public string Documenttype { get; set; }
		public string Documentpart { get; set; }
		public string Documentnumber { get; set; }

		public string ClassType { get; set; }
		public string ClassName { get; set; }
		public string CharName { get; set; }
		public string CharValue { get; set; }
	}
}