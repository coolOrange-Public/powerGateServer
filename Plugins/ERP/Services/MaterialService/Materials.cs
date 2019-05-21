using ErpServices.Database;
using ErpServices.Services.MaterialService.Entities;

namespace ErpServices.Services.MaterialService
{
	public class Materials : ContextEntitySetBase<Material, Material>
	{
		public Materials(IEntityStores entityStores) : base(entityStores)
		{
		}

		public override void Update(Material entity)
		{
			Throw(entity, "{0} has no property that can be updated because it has only keys! " +
				"For updating navigation properties please use the correct EntitySet!");
		}
	}
}