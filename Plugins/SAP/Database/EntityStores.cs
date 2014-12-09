using System;
using System.Collections.Generic;
using SapServices.Converters;

namespace SapServices.Database
{
	public interface IEntityStores
	{
		IEntityStore<T> AddStoreFor<T>();
		IEntityStore<T> AddStoreFor<T>(IEntityDbConverter<T> converter);
		IEntityStore<T> ResolveFor<T>();
	}
	public class EntityStores : IEntityStores
	{
		private readonly IDatabase _database;
		private readonly Dictionary<Type, object> _entityStores = new Dictionary<Type, object>();

		public EntityStores(IDatabase database)
		{
			_database = database;
		}

		public IEntityStore<T> AddStoreFor<T>()
		{
			return AddStoreFor(new ExtendedReflectionEntityDbConverter<T>(this));
		}

		public IEntityStore<T> AddStoreFor<T>(IEntityDbConverter<T> converter)
		{
			var entityStore = new EntityStore<T>(_database, converter);
			_entityStores.Add(typeof(T), entityStore);
			entityStore.Reload();
			return entityStore;
		}

		public IEntityStore<T> ResolveFor<T>()
		{
			var entityStore = _entityStores[typeof (T)];
			return  entityStore as IEntityStore<T>;
		} 
	}
}