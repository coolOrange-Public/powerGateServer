using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
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