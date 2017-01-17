using System;
using System.Data.SQLite;
using System.IO;
using IQToolkit.Data;
using IQToolkit.Data.Common;
using IQToolkit.Data.Mapping;
using IQToolkit.Data.SQLite;
using powerGateServer.SDK;

namespace LinqToDb
{
	[WebServiceData("LinqToDb","LINQ_SRV")]
	public class LinqToDbService : WebService
	{
		public LinqToDbService()
		{
			var dbConnection = OpenDatabaseConnection();
			DbEntityProvider entityProvider = new SQLiteQueryProvider(dbConnection, new ImplicitMapping(), new QueryPolicy());

			AddMethod(new Files(entityProvider));
		}

		SQLiteConnection OpenDatabaseConnection()
		{
			var dbFile = new FileInfo(GetDatabaseFolder().FullName + "\\LinqToDbService.sdf");
			var dbConnection = new SQLiteConnection(GetConnectionString(dbFile));
			dbConnection.Open();
			return dbConnection;
		}

		static string GetConnectionString(FileSystemInfo sdfFile)
		{
			return string.Format("Data Source={0};Max Database Size={1};PRAGMA synchronous=OFF",
				sdfFile.FullName, 4091);
		}

		DirectoryInfo GetDatabaseFolder()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			var specificFolder = Path.Combine(folder, @"coolorange\powerGateServer\Plugins\LinqToDb");

			return new DirectoryInfo(specificFolder);
		}
	}
}