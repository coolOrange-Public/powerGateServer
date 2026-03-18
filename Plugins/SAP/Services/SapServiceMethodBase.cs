using System;
using System.Collections.Generic;
using System.Linq;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;
using SapServices.Database;

namespace SapServices.Services
{
	public abstract class SapServiceMethodBase<T> : ServiceMethod<T>
	{
		protected readonly IEntityStores EntityStores;

		protected SapServiceMethodBase(IEntityStores entityStores)
		{
			EntityStores = entityStores;
		}

		public override IEnumerable<T> Query(IExpression<T> expression)
		{
			return EntityStore;
		}

		public override void Update(T entity)
		{
			Throw(entity, "{0} has no property that can be updated because it has only keys! " +
				"For updating navigation properties please use the correct EntitySet!");
		}

		public override void Create(T entity)
		{
			if (GetCurrentEntity(entity) != null)
				ThrowElementAllreadyExists(entity);
			EntityStore.Insert(entity);
		}

		public override void Delete(T entity)
		{
			var currentEntity = GetCurrentEntity(entity);
			if (currentEntity == null)
				ThrowElementDoesNotExists(entity);
			EntityStore.Delete(currentEntity);
		}

		protected IEntityStore<T> EntityStore
		{
			get { return EntityStores.ResolveFor<T>(); }
		}

		protected T GetCurrentEntity(T entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return EntityStore.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}

		protected void ThrowElementAllreadyExists(T entity)
		{
			Throw(entity, "A {0} with key: [{1}] allready exists!");
		}
		protected void ThrowElementDoesNotExists(T entity)
		{
			Throw(entity, "A {0} with key: [{1}] was not found!");
		}

		protected void Throw(T entity, string message)
		{
			throw new Exception(string.Format(message,
				typeof(T).Name,
				string.Join(";", entity.GetDataServiceKeys().Select(x => x.Key + "=" + x.Value).ToArray())));
		}
	}
}