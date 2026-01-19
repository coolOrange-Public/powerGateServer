using System.Collections.Generic;
using System.Linq;
using ErpServices.Database;
using ErpServices.Services.Entities;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;

namespace ErpServices.Services.Boms
{
	public class BomRows : ErpServiceMethodBase<BomRow>
	{
		IEntityStore<Item> _itemsStore
		{
			get { return EntityStores.ResolveFor<Item>(ClientId); }
		}

		public BomRows(IEntityStores entityStores) : base(entityStores)
		{
		}

		public override IEnumerable<BomRow> Query(IExpression<BomRow> expression)
		{
			var items = _itemsStore.ToArray();
			return EntityStore.Select(row => new BomRow(row) { Item = items.FirstOrDefault(item => item.Number == row.ChildNumber) });
		}

		public override void Create(BomRow entity)
		{
			if (entity.Item != null && GetItemEntity(entity.Item) != null)
				ThrowElementAlreadyExists(entity.Item);
			base.Create(entity);
			if(entity.Item != null)
				_itemsStore.Insert(entity.Item);
		}
		
		Item GetItemEntity(Item entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return _itemsStore.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}
	}
}