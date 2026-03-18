using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	public class PlantDataCollection : MaterialContextNavigationPropertyEntitySet<PlantData>
	{
		public override string Name
		{
			get { return "PlantDataCollection"; }
		}

		public PlantDataCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}
	}
}