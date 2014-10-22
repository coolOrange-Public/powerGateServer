using UserServices.Entities;

namespace UserServices.ServiceDefinition
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