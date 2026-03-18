using System;
using System.IO;
using System.Reflection;
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
		}

		protected abstract void CreateTablesForEntities();
		protected abstract void RegisterEntitySets();

		protected IServiceMethod AddEntitySetOfType<T>()
		{
			var entitySet = (IServiceMethod)Activator.CreateInstance(typeof(T), EntityStores);
			AddMethod(entitySet);
			return entitySet;
		}

		IDirectoryInfo GetDatabaseFolder()
		{
			var pluginDirPath = new System.IO.FileInfo(Assembly.GetAssembly(this.GetType()).Location).DirectoryName;
			var storeDirPath = Path.Combine(pluginDirPath, "Store");

			var dbFolder = new DirectoryInfo(storeDirPath);
			if (!dbFolder.Exists)
				dbFolder.Create();
			return dbFolder;
		}
	}
}