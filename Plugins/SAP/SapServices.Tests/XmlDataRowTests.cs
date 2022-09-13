using System.Data;
using System.Linq;
using System.Text;
using Autofac;
using NSubstitute;
using NUnit.Framework;
using SapServices.Database;
using SapServices.FileSystem;

namespace SapServices.Tests
{
	[TestFixture]
	public class XmlDataRowTests : ContainerBaseTest
	{

		[Test]
		public void Primarykey_isSetToPrimaryKeyColumnValue_from_db_row()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent(new[]
			{
				" <EntityID>666</EntityID>"
			});
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var table = Container.Resolve<XmlDbTable>();

			Assert.AreEqual(666, table.DataRows.First().Value.PrimaryKey);
		}

		[Test]
		public void Data_returns_each_row_entry_with_its_columnName()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent();
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var ds = new DataSet("TestTable");
			var dbTable = ds.Tables.Add("Test");
			dbTable.Columns.Add("EntityID", typeof(int));
			dbTable.Columns.Add("Col1", typeof (int));
			dbTable.Columns.Add("Col2", typeof(int));
			dbTable.Columns.Add("Col3", typeof(string));
			var row = dbTable.Rows.Add(666,1, 2, "This is a text");

			var table = Container.Resolve<XmlDbTable>();
			table.DataSet = ds;

			var dataRow = Container.Resolve<XmlDataRow>(
				new TypedParameter(typeof(XmlDbTable), table),
				new TypedParameter(typeof(DataRow), row));


			CollectionAssert.AreEqual(new[] { "EntityID", "Col1", "Col2", "Col3" }, dataRow.Data.Keys);
			CollectionAssert.AreEqual(new object[] { 666, 1, 2, "This is a text" }, dataRow.Data.Values);
		}

		[Test]
		public void Update_existing_field_value()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent();
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var ds = new DataSet("TestTable");
			var dbTable = ds.Tables.Add("Test");
			dbTable.Columns.Add("EntityID", typeof(int)).AutoIncrement=true;
			dbTable.Columns.Add("Col1", typeof(int));

			var table = Container.Resolve<XmlDbTable>();
			table.DataSet = ds;
			var row = table.CreateDataRow();
			row.Update("Col1",666);

			Assert.AreEqual(666,row.Data["Col1"]);
		}

		[Test]
		public void Update_column_that_doesnt_exists_creates_the_new_column_in_the_Table()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent();
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var table = Container.Resolve<XmlDbTable>();
			var row = table.CreateDataRow();
			row.Update("PlaPlaColumn",null);
			Assert.IsTrue(table.DataSet.Tables[0].Columns.Contains("PlaPlaColumn"));
		}

		[Test]
		public void Delete_removes_The_Correct_row_from_The_DataRows()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent();
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var table = Container.Resolve<XmlDbTable>();
			var row1 = table.CreateDataRow();
			var row2 = table.CreateDataRow();
			var row3 = table.CreateDataRow();
			row2.Delete();

			CollectionAssert.AreEqual(new[]{row1,row3},table.DataRows.Values);
		}

		[Test]
		public void Delete_removes_the_entity_really_from_the_dataSet()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			string xmlFileContentAfterCommit = string.Empty;
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			xmlFile.OpenWrite().Returns(TestData.GetDefaultXmlContent()
				.ToStream(stream =>
					xmlFileContentAfterCommit = string.Join("", stream.ReadAllLines(Encoding.UTF8))));
			
			var table = Container.Resolve<XmlDbTable>();
			table.CreateDataRow().Delete();
			table.Commit();
			Assert.AreEqual(TestData.GetDefaultXmlContent().Replace(" ", ""), xmlFileContentAfterCommit.Replace(" ", ""));
		}
	}
}
