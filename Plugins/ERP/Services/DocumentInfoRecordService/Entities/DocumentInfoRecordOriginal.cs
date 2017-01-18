using System;
using System.Data.Services.Common;
using powerGateServer.SDK;

namespace ErpServices.Services.DocumentInfoRecordService.Entities
{
	[DataServiceKey("Documenttype", "Documentnumber", "Documentpart", "Documentversion", "Description")]
	[DataServiceEntity]
	public class DocumentInfoRecordOriginal : Streamable
	{
		public string Documenttype { get; set; }
		public string Documentnumber { get; set; }
		public string Documentpart { get; set; }
		public string Documentversion { get; set; }
		public string Description { get; set; }
		public string Storagecategory { get; set; }
		public string Wsapplication { get; set; }
		public string MimeType { get; set; }
		public byte[] Value { get; set; }
		public string ApplicationID { get; set; }

		public DocumentInfoRecordOriginal() 
		{
			Value = new Byte[0];
			Storagecategory = "ZPDF";
			Wsapplication = "PDF";
			ApplicationID = "Default";
			FileName="CAD_Upload.pdf";
			MimeType = "";
		}

		public override string GetContentType()
		{
			return ContentTypes.Application.Pdf;
		}
	}
}