using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using VaultServices.Entities.Base;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Entities.File
{
	[DataServiceKey("Id")]
	[DataServiceEntity]
	public class File : BaseObject
	{
		public override sealed long Id { get; set; }
		public override sealed string Type { get; set; }
		public override sealed string Name { get; set; }
		public override sealed string CreateUser { get; set; }
		public override sealed DateTime CreateDate { get; set; }
		public override IEnumerable<Property.Property> Properties { get; set; }
		public override IEnumerable<Link.Link> Children { get; set; }
		public override IEnumerable<Link.Link> Parents { get; set; }

		public File()
		{
		}

		internal File(Vault.File file)
		{
			Id = file.Id;
			Name = file.Name;
			CreateUser = file.CreateUserName;
			CreateDate = file.CreateDate;
		}
	}
}