using System;
using System.IO;
using System.Linq;
using ErpServices.Database;
using ErpServices.FileSystem;
using NSubstitute;
using NUnit.Framework;
using DirectoryInfo = ErpServices.FileSystem.DirectoryInfo;

namespace ErpServices.Tests
{
	[TestFixture]
	public class XmlDatabaseTests : ContainerBaseTest
	{

		[Test]
		[Ignore]
		public void General_test_for_debugging_whole_Db_in_General()
		{
			var dbFolder = new DirectoryInfo(Path.GetTempPath() + "XmlTestDb");
			dbFolder.Create();
			var xmlDb = new XmlDatabase(dbFolder);

			var catTable = !xmlDb.Tables.ContainsKey("Category") 
				? xmlDb.CreateTable("Category") : 
				xmlDb.Tables["Category"];
			var dataRow = catTable.CreateDataRow();
			dataRow.Update("Test","pla");
			xmlDb.Commit();
		}

		[Test]
		public void dbFolder_hasNoXmlFiles_ThanNoTableIsSet()
		{
			Container.SubstituteFor<IDirectoryInfo>()
				.GetFiles(String.Empty, SearchOption.AllDirectories)
				.ReturnsForAnyArgs(Enumerable.Empty<IFileInfo>());

			var xmlDb = Container.Resolve<XmlDatabase>();

			CollectionAssert.IsEmpty(xmlDb.Tables);
		}

		[Test]
		public void dbFolder_hasTwoXmlFiles_TwoTablesWithTheNameOfTheXmlAreSet()
		{
			var xmlFile1 = Substitute.For<IFileInfo>();
			xmlFile1.FullName.Returns(@"C:\temp\Test1.xml");
			xmlFile1.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			var xmlFile2 = Substitute.For<IFileInfo>();
			xmlFile2.FullName.Returns(@"C:\temp\Test2.xml");
			xmlFile2.OpenRead().Returns(TestData.GetDefaultXmlContent().ToStream());
			Container.SubstituteFor<IDirectoryInfo>()
				.GetFiles(String.Empty, SearchOption.AllDirectories)
				.ReturnsForAnyArgs(new[] { xmlFile1 ,xmlFile2});

			var xmlDb = Container.Resolve<XmlDatabase>();
			CollectionAssert.AreEqual(new[]{"Test1","Test2"},xmlDb.Tables.Keys);
			CollectionAssert.AreEqual(new[] { "Test1", "Test2" }, xmlDb.Tables.Values.Select(t=>t.Name));
		}

		[Test]
		public void CreateTable_creates_and_adds_a_new_table()
		{
			var xmlFile = Substitute.For<IFileInfo>();
			xmlFile.FullName.Returns(@"C:\temp\Test.xml");
			xmlFile.OpenRead().Returns("<Test></Test>".ToStream());
			xmlFile.Create().Returns("".ToStream());
			Container.SubstituteFor<IDirectoryInfo>()
				.AddFile("Test.xml").Returns(xmlFile);

			var xmlDb = Container.Resolve<XmlDatabase>();
			var table = xmlDb.CreateTable("Test");

			CollectionAssert.Contains(xmlDb.Tables.Keys,"Test");
			Assert.AreSame(xmlDb.Tables.First().Value,table);
		}

		[Test]
		public void Commit_commits_changes_in_all_Tables()
		{
			var xmlDb = Container.Resolve<XmlDatabase>();
			xmlDb.Tables.Add("Test1",Substitute.For<IDbTable>());
			xmlDb.Tables.Add("Test2", Substitute.For<IDbTable>());
			xmlDb.Commit();

			xmlDb.Tables.Values.ToList().ForEach(t => t.Received().Commit());
		}
	}
}
