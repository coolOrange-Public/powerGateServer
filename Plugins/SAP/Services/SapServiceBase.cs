using System;
using System.IO;
using powerGateServer.SDK;
using SapServices.Database;
using SapServices.FileSystem;
using DirectoryInfo = SapServices.FileSystem.DirectoryInfo;

namespace SapServices.Services
{
	public abstract class SapServiceBase : WebService
	{

		protected readonly IEntityStores EntityStores;

		protected SapServiceBase()
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
			var specificFolder = Path.Combine(folder, @"coolorange\powerGateServer\Plugins\SAP\Store");

			var dbFolder = new DirectoryInfo(specificFolder);
			if (!dbFolder.Exists)
				dbFolder.Create();
			return dbFolder;
		}
	}
}