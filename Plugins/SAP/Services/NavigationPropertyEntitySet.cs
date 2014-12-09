using System.Linq;
using SapServices.Database;

namespace SapServices.Services
{
	public abstract class NavigationPropertyEntitySet<T, TParent> : ContextEntitySetBase<T, TParent>
	{
		protected NavigationPropertyEntitySet(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override void Create(T entity)
		{
			if (GetCurrentEntity(entity) != null)
				ThrowElementAllreadyExists(entity);

			ReSetEntity(entity, entity);
		}

		public override void Delete(T entity)
		{
			if (GetCurrentEntity(entity) == null)
				ThrowElementDoesNotExists(entity);

			ReSetEntity(entity, null);
		}

		public override void Update(T entity)
		{
			if (GetCurrentEntity(entity) == null)
				ThrowElementDoesNotExists(entity);

			ReSetEntity(entity, entity);
		}

		void ReSetEntity(T entityOrig, object entity)
		{
			var context = GetParentEntity(entityOrig);
			ContextStore.Delete(context);

			var propsToUpdate = context.GetType().GetProperties().Where(p => p.PropertyType == typeof(T) && p.CanWrite);
			foreach (var propertyInfo in propsToUpdate)
				propertyInfo.SetValue(context, entity, null);
			ContextStore.Insert(context);
		}

		protected TParent GetParentEntity(T entity)
		{
			var parentEntities = ContextStore.Where(m => IsParentFor(m,entity));
			if (!parentEntities.Any())
				Throw(entity, "Failed to find {0} with key: [{1}]!");
			if (parentEntities.Count() > 1)
				Throw(entity, "Multiple {0} where found with key: [{1}]!");
			return parentEntities.First();
		}

		protected abstract bool IsParentFor(TParent parent, T entity);
	}
}