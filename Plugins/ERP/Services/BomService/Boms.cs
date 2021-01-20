using ErpServices.Database;
using ErpServices.Services.BomService.Entities;

namespace ErpServices.Services.BomService
{
	public class Boms : ContextEntitySetBase<Bom, Bom>
	{
		public Boms(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override void Update(Bom entity)
		{
			Throw(entity, "{0} has no property that can be updated because it has only keys! " +
				"For updating navigation properties please use the correct EntitySet!");
		}
	}
}