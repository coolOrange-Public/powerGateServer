using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using IQToolkit.Data;
using IQToolkit.Data.Common;
using IQToolkit.Data.Mapping;
using IQToolkit.Data.SQLite;
using powerGateServer.Addins;

namespace UserServices.LinqToDatabase
{
	public class MaterialService : IWebService
	{
		public string Name { get { return "http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV"; } }
		
		public IEnumerable<IServiceMethod> Methods { get; private set; }

		public MaterialService()
		{
			var dbFile = new FileInfo(GetDatabaseFolder().FullName + "\\MaterialContextCollection.db3");
			var dbConnection = new SQLiteConnection(GetConnectionString(dbFile));
			DbEntityProvider entityProvider = new SQLiteQueryProvider(dbConnection, new ImplicitMapping(), new QueryPolicy());

			Methods = new List<IServiceMethod>
			{
				new MaterialContextCollection(entityProvider),
				new PlantDataCollection(entityProvider)
			};
		}

		static string GetConnectionString(FileSystemInfo sdfFile)
		{
			return string.Format("Data Source={0};Max Database Size={1};PRAGMA synchronous=OFF",
				sdfFile.FullName, 4091);
		}

		DirectoryInfo GetDatabaseFolder()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var specificFolder = Path.Combine(folder, "coolorange\\powerGateServer\\LinqDB");

			var dbFolder = new DirectoryInfo(specificFolder);
			if (!dbFolder.Exists)
				dbFolder.Create();
			return dbFolder;
		}
	}
}