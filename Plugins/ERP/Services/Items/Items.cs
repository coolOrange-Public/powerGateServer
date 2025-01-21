using ErpServices.Database;
using ErpServices.Services.Entities;

namespace ErpServices.Services.Items
{
	public class Items : ErpServiceMethodBase<Item>
	{
		public Items(IEntityStores entityStores) : base(entityStores)
		{
		}
	}
}