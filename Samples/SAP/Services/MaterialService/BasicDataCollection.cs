using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class BasicDataCollection : MaterialContextNavigationPropertyEntitySet<BasicData>
	{
		public override string Name
		{
			get { return "BasicDataCollection"; }
		}

		public BasicDataCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}
	}
}