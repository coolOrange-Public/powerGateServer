using System;
using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	public abstract class MaterialContextNavigationPropertyEntitySet<T> : NavigationPropertyEntitySet<T, MaterialContext>
	{
		protected MaterialContextNavigationPropertyEntitySet(IEntityStores entityStores)
			:base(entityStores)
		{
		}

		protected override bool IsParentFor(MaterialContext parent, T entity)
		{
			return new MaterialContextComparer(entity)
				.Equals(parent);
		}
	}
	public class MaterialContextComparer : IEquatable<MaterialContext>
	{
		private readonly dynamic _source;

		public MaterialContextComparer(dynamic source)
		{
			_source = source;
		}

		public bool Equals(MaterialContext context)
		{
			return Equals(context.Material, _source.Material);
		}
	}
}