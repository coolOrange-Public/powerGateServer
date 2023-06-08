using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using ErpServices.Database;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;

namespace ErpServices.Services
{
	public abstract class ErpServiceMethodBase<T> : ServiceMethod<T>
	{
		protected readonly IEntityStores EntityStores;

		internal Func<string> GetCurrentClientId = AuthenticatedUser.GetUsername;

		protected ErpServiceMethodBase(IEntityStores entityStores)
		{
			EntityStores = entityStores;
		}

		protected string ClientId
		{
			get { return GetCurrentClientId(); }
		}

		protected IEntityStore<T> EntityStore
		{
			get { return EntityStores.ResolveFor<T>(ClientId); }
		}

		public override IEnumerable<T> Query(IExpression<T> expression)
		{
			return EntityStore;
		}

		public override void Update(T entity)
		{
			Delete(entity);
			Create(entity);
		}

		public override void Create(T entity)
		{
			if (GetCurrentEntity(entity) != null)
				ThrowElementAlreadyExists(entity);
			EntityStore.Insert(entity);
		}

		public override void Delete(T entity)
		{
			var currentEntity = GetCurrentEntity(entity);
			if (currentEntity == null)
				ThrowElementDoesNotExists(entity);
			EntityStore.Delete(currentEntity);
		}

		protected T GetCurrentEntity(T entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return EntityStore.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}

		protected void ThrowElementAlreadyExists(T entity)
		{
			Throw(entity, "A {0} with key: [{1}] already exists!");
		}

		protected void ThrowElementDoesNotExists(T entity)
		{
			Throw(entity, "A {0} with key: [{1}] was not found!");
		}

		protected void Throw(T entity, string message)
		{
			throw new Exception(string.Format(message,
				typeof (T).Name,
				string.Join(";", entity.GetDataServiceKeys().Select(x => x.Key + "=" + x.Value).ToArray())));
		}
	}
	
	[ExcludeFromCodeCoverage]
	internal static class AuthenticatedUser
	{
		internal static string GetUsername()
		{
			if (ServiceSecurityContext.Current == null || ServiceSecurityContext.Current.PrimaryIdentity == null || ServiceSecurityContext.Current.PrimaryIdentity.Name == null)
				return string.Empty;
			return ServiceSecurityContext.Current.PrimaryIdentity.Name;
		}
	}
}