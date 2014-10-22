using System;
using System.Collections.Generic;
using System.IO;
using powerGateServer.Addins;
using UserServices.Database;
using UserServices.FileSystem;
using UserServices.Services.MaterialService;
using DirectoryInfo = UserServices.FileSystem.DirectoryInfo;

namespace UserServices.Services
{
	public abstract class SapServiceBase : IWebService
	{
		public abstract string Name { get; }

		readonly IList<IServiceMethod> _methods = new List<IServiceMethod>();
		public IEnumerable<IServiceMethod> Methods { get { return _methods; } }
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
			_methods.Add(entitySet);
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
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var specificFolder = Path.Combine(folder, "coolorange\\powerGateServer\\SAP\\Store");

			var dbFolder = new DirectoryInfo(specificFolder);
			if (!dbFolder.Exists)
				dbFolder.Create();
			return dbFolder;
		}
	}
}