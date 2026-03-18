using System;
using System.Collections.Generic;
using ErpServices.Converters;

namespace ErpServices.Database
{
	public interface IEntityStores
	{
		void AddStoreFor<T>();
		void AddStoreFor<T>(IEntityDbConverter<T> converter);
		IEntityStore<T> ResolveFor<T>(string clientId);
		
	}
	public class EntityStores : IEntityStores
	{
		readonly IDatabase _database;
		readonly IDictionary<Type, object> _registeredStores = new Dictionary<Type, object>();
		readonly IDictionary<string, IDictionary<Type, object>> _entityStores = new Dictionary<string, IDictionary<Type, object>>();

		public EntityStores(IDatabase database)
		{
			_database = database;
		}

		public void AddStoreFor<T>()
		{
			AddStoreFor(new ReflectionEntityDbConverter<T>());
		}

		public void AddStoreFor<T>(IEntityDbConverter<T> converter)
		{
			_registeredStores.Add(typeof(T), converter);
		}

		public IEntityStore<T> ResolveFor<T>(string clientId)
		{
			if(!_entityStores.ContainsKey(clientId))
				_entityStores.Add(clientId, new Dictionary<Type, object>());
			
			if (_entityStores[clientId].ContainsKey(typeof(T)))
			{
				var entityStore = _entityStores[clientId][typeof(T)] as IEntityStore<T>;
				entityStore.Reload();
				return entityStore;
			}
			
			return CreateAndAddEntityStore<T>(clientId);
		}

		IEntityStore<T> CreateAndAddEntityStore<T>(string clientId)
		{
			var entityStore = new EntityStore<T>(_database, _registeredStores[typeof(T)] as IEntityDbConverter<T>, clientId);
			_entityStores[clientId].Add(typeof(T), entityStore);
			entityStore.Reload();
			return entityStore;
		}
	}
}