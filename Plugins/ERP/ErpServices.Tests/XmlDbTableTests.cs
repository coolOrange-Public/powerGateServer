using System.Linq;
using System.Text;
using ErpServices.Database;
using ErpServices.FileSystem;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class XmlDbTableTests : ContainerBaseTest
	{

		[Test]
		public void Constructor_setsTheTableName_to_Xml_FileName()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Sample.xml");
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			var table = Container.Resolve<XmlDbTable>();
			Assert.AreEqual("Sample", table.Name);
		}

		[Test]
		public void When_passingDefault_xmlFile_Table_has_not_rows()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Sample.xml");
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			var table = Container.Resolve<XmlDbTable>();
			CollectionAssert.IsEmpty(table.DataRows);
		}

		[Test]
		public void When_passing_xmlFile_WithSomeRows_Table_has_this_rows()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Sample.xml");
			var xmlFileContent = TestData.GetDefaultXmlContent(new[]
			{
				" <EntityID>0</EntityID><SomeThing>pla</SomeThing>",
				" <EntityID>1</EntityID><SomeThing>10001</SomeThing>",
				" <EntityID>2</EntityID><SomeThing>10006</SomeThing>"
			});
			xmlFile.OpenRead().Returns(xmlFileContent.ToStream());
			var table = Container.Resolve<XmlDbTable>();
			CollectionAssert.AreEqual(new[]{0,1,2},table.DataRows.Keys);
			CollectionAssert.AreEqual(new[] { 0, 1, 2 },table.DataRows.Values.Select(r=>r.PrimaryKey));
		}

		[Test]
		public void CreateDataRow_adds_and_creates_a_new_DataRow_to_Table()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Sample.xml");
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());

			var table = Container.Resolve<XmlDbTable>();
			var row = table.CreateDataRow();

			CollectionAssert.AreEqual(new[] { 0 }, table.DataRows.Keys);
			CollectionAssert.AreEqual(new[] { row }, table.DataRows.Values);
		}

		[Test]
		public void CreateDataRow_whenCallingItThreeTimes_returnsRowsWithAutoincrementedPrimaryKey()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Sample.xml");
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			
			var table = Container.Resolve<XmlDbTable>();

			Assert.AreEqual(0, table.CreateDataRow().PrimaryKey);
			Assert.AreEqual(1, table.CreateDataRow().PrimaryKey);
			Assert.AreEqual(2, table.CreateDataRow().PrimaryKey);
		}

		[Test]
		public void CreateDataRow_addsReally_a_new_dataRow_to_the_dataSet()
		{
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			string xmlFileContentAfterCommit = string.Empty;
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			xmlFile.OpenWrite().Returns(TestData.GetDefaultXmlContent()
				.ToStream(stream => 
					xmlFileContentAfterCommit = string.Join("",stream.ReadAllLines(Encoding.UTF8))));
			var table = Container.Resolve<XmlDbTable>();
			table.CreateDataRow();
			table.Commit();
			Assert.AreEqual(TestData.GetDefaultXmlContent(new[] { "<EntityID>0</EntityID>" }).Replace(" ", ""), xmlFileContentAfterCommit.Replace(" ", ""));
		}

		[Test]
		public void Commit_after_adding_a_new_row_the_row_is_written_to_stream()
		{
			string committedStreamText = string.Empty;
			var xmlFile = Container.SubstituteFor<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\Temp\Test.xml");
			xmlFile.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			xmlFile.OpenWrite().Returns(TestData.GetDefaultXmlContent().ToStream(stream =>
			{
				committedStreamText = string.Join("",stream.ReadAllLines(Encoding.UTF8));
			}));

			var table = Container.Resolve<XmlDbTable>();
			var row = table.CreateDataRow();
			table.Commit();
			
			//creating a second object with the result of stream after committing
			xmlFile.OpenRead().Returns(committedStreamText.ToStream());
			var tableAfterCommit = Container.Resolve<XmlDbTable>();
			//now there should be the same row that was committed before
			Assert.AreEqual(1,tableAfterCommit.DataRows.Count);
			Assert.AreEqual(row.PrimaryKey, tableAfterCommit.DataRows.First().Value.PrimaryKey);
		}
	}
}
