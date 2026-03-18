using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
	public abstract class BomNavigationPropertyCollectionEntitySet<T> 
		: NavigationPropertyCollectionEntitySet<T, BillOfMaterialContext>
	{
        protected BomNavigationPropertyCollectionEntitySet(IEntityStores entityStores)
            : base(entityStores)
		{
		}

		protected override bool IsParentFor(BillOfMaterialContext parent, T entity)
		{
			return new BillOfMaterialContextComparer(entity)
				.Equals(parent);
		}
	}
}