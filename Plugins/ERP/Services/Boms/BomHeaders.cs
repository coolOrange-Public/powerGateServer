using System.Collections.Generic;
using System.Linq;
using ErpServices.Database;
using ErpServices.Services.Entities;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;

namespace ErpServices.Services.Boms
{
	public class BomHeaders : ErpServiceMethodBase<BomHeader>
	{
		IEntityStore<BomRow> _bomRowsStore
		{
			get { return EntityStores.ResolveFor<BomRow>(ClientId); }
		}

		IEntityStore<Item> _itemsStore
		{
			get { return EntityStores.ResolveFor<Item>(ClientId); }
		}

		public BomHeaders(IEntityStores entityStores) : base(entityStores)
		{
		}

		public override IEnumerable<BomHeader> Query(IExpression<BomHeader> expression)
		{
			return EntityStore.Select(header => new BomHeader(header)
			{
				Children = _bomRowsStore.Where(row => row.ParentNumber == header.Number).Select(row => new BomRow(row)
				{
					Item = _itemsStore.FirstOrDefault(item => item.Number == row.ChildNumber)
				}).ToList()
			});
		}

		public override void Create(BomHeader entity)
		{
			if (entity.Children != null)
				ThrowIfBomRowOrItemAlreadyExists(entity);
			base.Create(entity);
			if (entity.Children != null)
				CreateBomRows(entity);
		}

		void ThrowIfBomRowOrItemAlreadyExists(BomHeader bomHeader)
		{
			foreach (var bomRow in bomHeader.Children)
			{
				if (GetBomRowEntity(bomRow) == null)
				{
					if (bomRow.Item != null && GetItemEntity(bomRow.Item) != null)
						ThrowElementAlreadyExists(bomRow.Item);
				}
				else
					ThrowElementAlreadyExists(bomRow);
			}
		}

		void CreateBomRows(BomHeader bomHeader)
		{
			foreach (var bomRow in bomHeader.Children)
			{
				_bomRowsStore.Insert(bomRow);
				if (bomRow.Item != null)
					_itemsStore.Insert(bomRow.Item);
			}
		}

		BomRow GetBomRowEntity(BomRow entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return _bomRowsStore.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}

		Item GetItemEntity(Item entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return _itemsStore.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}
	}
}