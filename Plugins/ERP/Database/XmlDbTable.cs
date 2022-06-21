using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using ErpServices.FileSystem;

namespace ErpServices.Database
{
	public class XmlDbTable : IDbTable
	{
		protected IFileInfo XmlFile;
		internal DataSet DataSet;
		
		public string Name { get; private set; }
		public string PrimaryKeyColumn { get { return "EntityID"; } }

		public Dictionary<int, IDbDataRow> DataRows { get; private set; }

		public XmlDbTable(IFileInfo xmlFile)
		{
			Name = Path.GetFileNameWithoutExtension(xmlFile.FullName);
			DataRows = new Dictionary<int, IDbDataRow>();
			XmlFile = xmlFile;
			DataSet = new DataSet();
			Initialize();
		}

		protected virtual void Initialize()
		{
			using (var fileStream = XmlFile.OpenRead())
				DataSet.ReadXml(fileStream, XmlReadMode.ReadSchema);
			var view = DataSet.Tables[0].DefaultView;
			foreach (var dataRow in view.ToTable().Rows.Cast<DataRow>())
			{
				var xmlDataRow = new XmlDataRow(this, dataRow);
				DataRows[xmlDataRow.PrimaryKey] = xmlDataRow;
			}
		}

		public IDbDataRow CreateDataRow()
		{
			var xmlDataRow = new NewXmlDataRow(this);
			return DataRows[xmlDataRow.PrimaryKey] = xmlDataRow;
		}

		public void Commit()
		{
			using (var fileStream = XmlFile.OpenWrite())
			{
				fileStream.SetLength(0);
				fileStream.Flush();
				DataSet.WriteXml(fileStream, XmlWriteMode.WriteSchema);
			}
		}
	}

	public class NewXmlDbTable : XmlDbTable
	{
		public NewXmlDbTable(IFileInfo xmlFile)
			: base(xmlFile)
		{
		}

		protected override void Initialize()
		{
			CreateXmlFile();
			using (var fileStream = XmlFile.OpenRead())
				DataSet.ReadXml(fileStream, XmlReadMode.IgnoreSchema);
			DataSet.DataSetName = Name + "Table";
			AddPrimaryKeyColumn();
		}

		void AddPrimaryKeyColumn()
		{
			var table = new DataTable(Name);
			table.Columns.Add(new DataColumn(PrimaryKeyColumn, typeof(int))
			{
				AutoIncrement = true
			});
			DataSet.Tables.Add(table);
		}

		void CreateXmlFile()
		{
			using (var fileStream = XmlFile.Create())
			using (var xmlWriter = XmlWriter.Create(fileStream))
			{
				xmlWriter.WriteStartElement(Name);
				xmlWriter.WriteEndElement();
			}
		}
	}
}