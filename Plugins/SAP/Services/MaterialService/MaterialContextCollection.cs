using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	public class MaterialContextCollection : ContextEntitySetBase<MaterialContext, MaterialContext>
	{
		public MaterialContextCollection(IEntityStores entityStores) : base(entityStores)
		{
		}

		public override string Name
		{
			get { return "MaterialContextCollection"; }
		}

		public override void Update(MaterialContext entity)
		{
			Throw(entity, "{0} has no property that can be updated because it has only keys! " +
				"For updating navigation properties please use the correct EntitySet!");
		}
	}
}