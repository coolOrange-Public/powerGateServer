using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;

namespace UserServices.Vault.Entities
{
	[DataServiceKey("Id")]
	[DataServiceEntity]
	public partial class File
	{
		public int Id { get; set; }
		public int MasterId { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Classification { get; set; }
		public string State { get; set; }
		public int Version { get; set; }
		public string Revision { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public string CreatedBy { get; set; }
		public IEnumerable<Property> FileProperties { get; set; }
		public int CatColor { get; set; }
		public string Title { get { return GetPropertyValue("Title").ToString(); } }
		public string PartNumber { get { return GetPropertyValue("PartNumber").ToString(); } }
		public byte[] Thumbnail
		{
			get
			{
				return GetPropertyValue("Thumbnail") as byte[];
			}
		}

		public File()
		{
			FileProperties = new List<Property>();
		}

		object GetPropertyValue(string propertyName)
		{
			var prop = FileProperties.FirstOrDefault(f => f.Name.Equals(propertyName));
			if (prop == null || prop.Value == null)
				return string.Empty;
			return prop.Value;
		}
	}
}