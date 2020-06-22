using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	public class ValuationDataCollection : MaterialContextNavigationPropertyEntitySet<ValuationData>
	{
		public override string Name
		{
			get { return "ValuationDataCollection"; }
		}

		public ValuationDataCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}
	}
}