using ErpServices.Database;
using ErpServices.Services.MaterialService.Entities;

namespace ErpServices.Services.MaterialService
{
	public class MaterialDescriptions : NavigationPropertyCollectionEntitySet<MaterialDescription, Material>
	{
		public MaterialDescriptions(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		protected override bool IsParentFor(Material parent, MaterialDescription entity)
		{
			return new DynamicMaterialComparer(entity)
				.Equals(parent);
		}
	}
}