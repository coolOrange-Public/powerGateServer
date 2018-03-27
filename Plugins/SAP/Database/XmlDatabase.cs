using System.Collections.Generic;
using System.IO;
using System.Linq;
using SapServices.FileSystem;

namespace SapServices.Database
{
	public class XmlDatabase : IDatabase
	{
		private readonly IDirectoryInfo _dbFolder;
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
				Tables[table.Name] = table;
		}

		public IDbTable CreateTable(string name)
		{
			var xmlFile = _dbFolder.AddFile(name + ".xml");
			return Tables[name] = new NewXmlDbTable(xmlFile);
		}

		public void Commit()
		{
			foreach (var table in Tables.Values)
				table.Commit();
		}
	}
}
