using UserServices.Entities;

namespace UserServices.ServiceDefinition
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