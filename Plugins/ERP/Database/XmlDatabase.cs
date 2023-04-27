using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErpServices.FileSystem;

namespace ErpServices.Database
{
	public class XmlDatabase : IDatabase
	{
		readonly IDirectoryInfo _dbFolder;
		public Dictionary<string, IDbTable> Tables { get; private set; }

		public XmlDatabase(IDirectoryInfo dbFolder)
		{
			_dbFolder = dbFolder;
			Tables = new Dictionary<string, IDbTable>();
			Reload();
		}

		public void Reload()
		{
			Tables.Clear();
			var xmlFiles = _dbFolder.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
			foreach (var table in xmlFiles.Select(xmlFile => new XmlDbTable(xmlFile)))
			{
				table.Initialize();
				Tables[table.Filename] = table;
			}
		}

		public IDbTable CreateTable(string name, string fileName)
		{
			var xmlFile = _dbFolder.AddFile(fileName + ".xml");
			var dbTable = new NewXmlDbTable(xmlFile, name);
			dbTable.Initialize();
			return Tables[fileName] = dbTable;
		}

		public IDbTable GetTable(string fileName)
		{
			if (!Tables.ContainsKey(fileName))
				return null;
			var xmlFiles = _dbFolder.GetFiles(fileName + ".xml", SearchOption.TopDirectoryOnly);
			if (xmlFiles == null || !xmlFiles.Any())
			{
				Tables.Remove(fileName);
				return null;
			}
			return Tables[fileName];
		}
		
		public void Commit()
		{
			foreach (var table in Tables.Values)
				table.Commit();
		}
	}
}
