using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SapServices.Converters;
using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Tests
{
	[TestFixture]
	public class EntityStoreTests : ContainerBaseTest
	{

		[Test]
		public void Reload_creates_a_table_for_the_entity()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.Tables.Returns(new Dictionary<string, IDbTable>());
			Container.Provide<IEntityDbConverter<MaterialContext>>(new ReflectionEntityDbConverter<MaterialContext>());
			var entityStore = Container.Resolve<EntityStore<MaterialContext>>();

			entityStore.Reload();

			db.Received().CreateTable("MaterialContext");
		}

		[Test]
		public void Reload_ifTableExistsAllready_doesNotCreateAnewTable()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.Tables.Returns(new Dictionary<string, IDbTable>
			{
				{ "MaterialContext",Substitute.For<IDbTable>() }
			});
			var entityStore = Container.Resolve<EntityStore<MaterialContext>>();

			entityStore.Reload();

			db.DidNotReceiveWithAnyArgs().CreateTable(string.Empty);
		}

		[Test]
		public void Reload_addsForeachDataRow_a_new_Element()
		{
			var data1 = Substitute.For<IDbDataRow>();
			data1.PrimaryKey.Returns(0);
			data1.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 1}, 
				{"Name", "Elhanan Goro"},
				{"Id",666}
			});
			var data2 = Substitute.For<IDbDataRow>();
			data2.PrimaryKey.Returns(1);
			data2.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 2}, 
				{"Name", "Maquinna Sofia"},
				{"Id",999}
			});
			var table = Substitute.For<IDbTable>();
			table.DataRows.Returns(new Dictionary<int, IDbDataRow>
			{
				{0,data1},{1,data2}
			});
			var db = Container.SubstituteFor<IDatabase>();
			db.Tables.Returns(new Dictionary<string, IDbTable>{{ "TestEntity", table}});
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>();

			entityStore.Reload();

			Assert.AreEqual(2,entityStore.Count());
			Assert.IsTrue(entityStore.First().Name == "Elhanan Goro" && entityStore.First().Id == 666);
			Assert.IsTrue(entityStore.Last().Name == "Maquinna Sofia" && entityStore.Last().Id == 999);
		}

		[Test]
		public void Insert_Creates_and_Adds_A_new_entity_to_the_store()
		{
			var newRow = Substitute.For<IDbDataRow>();
			var table = Substitute.For<IDbTable>();
			table.CreateDataRow().Returns(newRow);
			var db = Container.SubstituteFor<IDatabase>();
			db.Tables.Returns(new Dictionary<string, IDbTable>
			{
				{ "TestEntity", table}
			});
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>();
			var entity = new TestEntity{Id = 666, Name = "Wanjala Catina" };

			entityStore.Insert(entity);

			CollectionAssert.IsNotEmpty(entityStore);
			newRow.ReceivedWithAnyArgs(2).Update("",null);
			newRow.Received().Update("Name", "Wanjala Catina");
			newRow.Received().Update("Id", 666);
		}

		[Test]
		public void Delete_whenRemoving_deletesTheEntityItself_andRemovesItFromTheStore()
		{
			var data = Substitute.For<IDbDataRow>();
			data.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 1}, 
				{"Name", "Marjolein Aage"},
				{"Id",666}
			});

			var table = Substitute.For<IDbTable>();
			table.DataRows.Returns(new Dictionary<int, IDbDataRow> { { 0, data } });
			var db = Container.SubstituteFor<IDatabase>();
			db.Tables.Returns(new Dictionary<string, IDbTable> { { "TestEntity", table } });
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>();
			entityStore.Reload();

			entityStore.Delete(entityStore.First());

			data.Received().Delete();
			CollectionAssert.IsEmpty(entityStore);
		}

		public class TestEntity
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}
