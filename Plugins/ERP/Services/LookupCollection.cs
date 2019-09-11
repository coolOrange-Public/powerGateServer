using ErpServices.Database;

namespace ErpServices.Services
{
	public class LookupCollection<T> : ErpServiceMethodBase<T>
	{
		public LookupCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return string.Format("{0}sLookup", typeof(T).Name.Replace("Lookup", string.Empty)); }
		}
	}
}