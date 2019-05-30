using System.Data.Services.Common;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using VaultServices.Entities.Base;

namespace VaultServices.Entities.Property
{
    [DataServiceKey("ParentId", "ParentType", "Name")]
    [DataServiceEntity]
    public class Property
    {
        public long ParentId { get; set; }
        public string ParentType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

	    public Property()
	    {
	    }

		internal Property(IBaseObject baseObject, IProperty property)
		{
			ParentId = baseObject.Id;
			ParentType = baseObject.Type;
			Name = property.DisplayName;
			Value = property.Value as string;
			Type = property.Definition.DataType.ToString();
		}
    }
}