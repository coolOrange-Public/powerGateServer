using System;
using System.IO;
using ErpServices.Database;
using ErpServices.FileSystem;
using powerGateServer.SDK;
using DirectoryInfo = ErpServices.FileSystem.DirectoryInfo;

namespace ErpServices.Services
{
	public abstract class ErpServiceBase : WebService
	{

		protected readonly IEntityStores EntityStores;

		protected ErpServiceBase()
		{
			var database = new XmlDatabase(GetDatabaseFolder());
			EntityStores = new EntityStores(database);

			CreateTablesForEntities();
			RegisterEntitySets();
			RegisterLookups();
		}

		protected abstract void CreateTablesForEntities();
		protected abstract void RegisterEntitySets();
		protected abstract void RegisterLookups();

		protected IServiceMethod AddEntitySetOfType<T>()
		{
			var entitySet = (IServiceMethod)Activator.CreateInstance(typeof(T), EntityStores);
			AddMethod(entitySet);
			return entitySet;
		}

		protected IServiceMethod AddLookupFor<T>()
		{
			return AddLookupFor<LookupCollection<T>, T>();
		}

		protected IServiceMethod AddLookupFor<TServiceMethod, TEntity>() 
			where TServiceMethod : LookupCollection<TEntity>
		{
			EntityStores.AddStoreFor<TEntity>();
			return AddEntitySetOfType<TServiceMethod>();
		}

		IDirectoryInfo GetDatabaseFolder()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			var specificFolder = Path.Combine(folder, @"coolorange\powerGateServer\Plugins\ERP\Store");

			var dbFolder = new DirectoryInfo(specificFolder);
			if (!dbFolder.Exists)
				dbFolder.Create();
			return dbFolder;
		}
	}
}