using System;
using System.Collections.Generic;
using System.Data.Services.Common;

namespace UserServices.Entities
{
	[DataServiceKey("ID")]
	public class Catalog
	{
		public string Url { get; set; }
		public DateTime UpdatedDate { get; set; }
		public string ImageUrl { get; set; }
		public string ID { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public IEnumerable<Service> Services { get; set; }
	}
}