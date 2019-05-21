using ErpServices.Database;
using ErpServices.Services.BomService.Entities;

namespace ErpServices.Services.BomService
{
	public class BomItems : NavigationPropertyCollectionEntitySet<BomItem, Bom>
	{
		public BomItems(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		protected override bool IsParentFor(Bom parent, BomItem entity)
		{
			return new DynamicBomComparer(entity)
							.Equals(parent);
		}
	}
}