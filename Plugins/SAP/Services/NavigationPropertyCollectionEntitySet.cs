using System.Collections.Generic;
using System.Linq;
using SapServices.Database;

namespace SapServices.Services
{
	public abstract class NavigationPropertyCollectionEntitySet<T, TParent> : NavigationPropertyEntitySet<T, TParent>
	{
		protected NavigationPropertyCollectionEntitySet(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override void Create(T entity)
		{
			if (GetCurrentEntity(entity) != null)
				ThrowElementAllreadyExists(entity);

			var context = GetParentEntity(entity);
			var collection = new List<T>(GetCollection(context));
			collection.Add(entity);
			ReSetEntity(context, collection);
		}

		public override void Delete(T entity)
		{
			var currentEntity = GetCurrentEntity(entity);
			if (currentEntity == null)
				ThrowElementDoesNotExists(entity);

			var dirContext = GetParentEntity(entity);
			var recordLinks = new List<T>(GetCollection(dirContext));
			recordLinks.Remove(currentEntity);
			ReSetEntity(dirContext, recordLinks);
		}

		public override void Update(T entity)
		{
			Delete(entity);
			Create(entity);
		}

		IEnumerable<T> GetCollection(TParent context)
		{
			var collectionProp = context.GetType().GetProperties()
				.FirstOrDefault(p => p.PropertyType == typeof(IEnumerable<T>) && p.CanWrite);
			return (IEnumerable<T>)collectionProp.GetValue(context, null);
		}

		void SetCollection(TParent context, IEnumerable<T> entities)
		{
			var collectionProp = context.GetType().GetProperties()
				.FirstOrDefault(p => p.PropertyType == typeof(IEnumerable<T>) && p.CanWrite);
			collectionProp.SetValue(context, entities, null);
		}

		private void ReSetEntity(TParent context, IEnumerable<T> entities)
		{
			ContextStore.Delete(context);
			SetCollection(context, entities);
			ContextStore.Insert(context);
		}
	}
}