using System.Data.Services.Common;

namespace UserServices.Vault.Entities
{
	[DataServiceKey("EntityId", "Name")]
	[DataServiceEntity]
	public class Property
	{
		public long EntityId { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}