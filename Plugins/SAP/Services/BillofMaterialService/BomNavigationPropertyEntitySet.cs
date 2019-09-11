using System;
using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
	public abstract class BomNavigationPropertyEntitySet<T> : NavigationPropertyCollectionEntitySet<T, BillOfMaterialContext>
	{
		protected BomNavigationPropertyEntitySet(IEntityStores entityStores)
			:base(entityStores)
		{
		}

		protected override bool IsParentFor(BillOfMaterialContext parent, T entity)
		{
			return new BillOfMaterialContextComparer(entity)
				.Equals(parent);
		}
	}

	public class BillOfMaterialContextComparer : IEquatable<BillOfMaterialContext>
	{
		private readonly dynamic _source;

		public BillOfMaterialContextComparer(dynamic source)
		{
			_source = source;
		}

		public bool Equals(BillOfMaterialContext context)
		{
			return Equals(context.Plant, _source.Plant) &&
				Equals(context.BOMUsage, _source.BOMUsage) &&
				Equals(context.Material, _source.Material) &&
				Equals(context.Alternative, _source.Alternative);
		}
	}
}