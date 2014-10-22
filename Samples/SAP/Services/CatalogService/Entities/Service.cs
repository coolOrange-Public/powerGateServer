using System;
using System.Data.Services.Common;

namespace UserServices.Entities
{
	[DataServiceKey("ID")]
	public class Service
	{
		public string ID { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int TechnicalServiceVersion { get; set; }
		public string MetadataUrl { get; set; }
		public string TechnicalServiceName { get; set; }
		public string ImageUrl { get; set; }
		public string ServiceUrl { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
