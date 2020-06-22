using System.Collections.Generic;
using System.Data.Services.Common;

namespace SapServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documentversion", "Documenttype", "Documentpart", "Documentnumber")]
	[DataServiceEntity]
	public class DocumentInfoRecordContext
	{
		public string Documentversion { get; set; }
		public string Documenttype { get; set; }
		public string Documentpart { get; set; }
		public string Documentnumber { get; set; }
		public DocumentInfoRecordData DocumentInfoRecordData { get; set; }
		public IEnumerable<DocumentInfoRecordOriginal> DocumentInfoRecordOriginals { get; set; }
		public IEnumerable<DocumentInfoRecordObjectLink> DocumentInfoRecordObjectLinks { get; set; }
		public IEnumerable<Classification> Classification { get; set; }
		public IEnumerable<CharacteristicValues> CharacteristicValues { get; set; }
        public IEnumerable<DocumentInfoRecordDescription> DocumentInfoRecordDescription { get; set; }

		public DocumentInfoRecordContext()
		{
			DocumentInfoRecordOriginals = new List<DocumentInfoRecordOriginal>();
			DocumentInfoRecordObjectLinks = new List<DocumentInfoRecordObjectLink>();
			Classification = new List<Classification>();
			CharacteristicValues = new List<CharacteristicValues>();
		    DocumentInfoRecordDescription = new List<DocumentInfoRecordDescription>();
		}
	}
}