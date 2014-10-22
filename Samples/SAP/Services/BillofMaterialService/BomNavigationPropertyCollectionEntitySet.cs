using UserServices.ServiceDefinition;
using UserServices.Services.Entities;

namespace UserServices.Services
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