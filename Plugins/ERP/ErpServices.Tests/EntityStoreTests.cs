using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Autofac;
using ErpServices.Converters;
using ErpServices.Database;
using ErpServices.Services.Entities;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class EntityStoreTests : ContainerBaseTest
	{

		[Test]
		public void Reload_creates_a_table_with_no_clientId_for_default_store_of_entity_when_clientId_is_empty()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable(Arg.Any<string>()).Returns((IDbTable)null);
			Container.Provide<IEntityDbConverter<Item>>(new ReflectionEntityDbConverter<Item>());
			
			var entityStore = Container.Resolve<EntityStore<Item>>(new TypedParameter(typeof(string), string.Empty));
			entityStore.Reload();

			db.Received().CreateTable("Item", "Item");
		}

		[Test]
		public void Reload_creates_table_with_client_id_in_name_when_not_empty()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable(Arg.Any<string>()).Returns((IDbTable)null);
			Container.Provide<IEntityDbConverter<Item>>(new ReflectionEntityDbConverter<Item>());
			
			var entityStore = Container.Resolve<EntityStore<Item>>(new TypedParameter(typeof(string), "myClientIdgoeshereandwild"));
			entityStore.Reload();

			db.Received().CreateTable("Item", "myClientIdgoeshereandwild_Item");
		}

		[Test]
		public void Reload_does_not_create_new_table_if_it_already_exists()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable(Arg.Any<string>()).Returns(Substitute.For<IDbTable>());
			
			var entityStore = Container.Resolve<EntityStore<Item>>(new TypedParameter(typeof(string), string.Empty));
			entityStore.Reload();

			db.DidNotReceiveWithAnyArgs().CreateTable(string.Empty, string.Empty);
		}

		[Test]
		public void Reload_does_not_create_new_table_in_clientId_bound_store_if_it_already_exists()
		{
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable("muchCustom_verystore_Item").Returns(Substitute.For<IDbTable>());
			
			var entityStore = Container.Resolve<EntityStore<Item>>(new TypedParameter(typeof(string), "muchCustom_verystore"));
			entityStore.Reload();

			db.Received(1).GetTable(Arg.Is("muchCustom_verystore_Item"));
			db.DidNotReceiveWithAnyArgs().CreateTable(string.Empty, string.Empty);
		}

		[Test]
		public void Reload_adds_for_each_dataRow_a_new_element()
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
				{0, data1}, {1, data2}
			});
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable(Arg.Any<string>()).Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string), string.Empty));
			entityStore.Reload();

			Assert.AreEqual(2,entityStore.Count());
			Assert.IsTrue(entityStore.First().Name == "Elhanan Goro" && entityStore.First().Id == 666);
			Assert.IsTrue(entityStore.Last().Name == "Maquinna Sofia" && entityStore.Last().Id == 999);
		}
		
		[Test]
		public void Reload_adds_entity_for_each_dataRow_of_clientId_bound_store_when_not_empty()
		{
			var data1 = Substitute.For<IDbDataRow>();
			data1.PrimaryKey.Returns(0);
			data1.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 1}, 
				{"Name", "hohoho"},
				{"Id",123}
			});
			var data2 = Substitute.For<IDbDataRow>();
			data2.PrimaryKey.Returns(1);
			data2.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 2}, 
				{"Name", "hehehe"},
				{"Id",321}
			});
			var table = Substitute.For<IDbTable>();
			table.DataRows.Returns(new Dictionary<int, IDbDataRow>
			{
				{0, data1}, {1, data2}
			});
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable("myId_and_onlyMine_TestEntity").Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string),"myId_and_onlyMine"));
			entityStore.Reload();

			Assert.AreEqual(2,entityStore.Count());
			Assert.IsTrue(entityStore.First().Name == "hohoho" && entityStore.First().Id == 123);
			Assert.IsTrue(entityStore.Last().Name == "hehehe" && entityStore.Last().Id == 321);
		}

		[Test]
		public void Insert_creates_and_adds_a_new_entity_to_the_store()
		{
			var newRow = Substitute.For<IDbDataRow>();
			var table = Substitute.For<IDbTable>();
			table.CreateDataRow().Returns(newRow);
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable(Arg.Any<string>()).Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string), string.Empty));
			var entity = new TestEntity{Id = 666, Name = "Wanjala Catina" };

			entityStore.Insert(entity);

			CollectionAssert.IsNotEmpty(entityStore);
			newRow.ReceivedWithAnyArgs(2).Update("",null);
			newRow.Received().Update("Name", "Wanjala Catina");
			newRow.Received().Update("Id", 666);
		}
		
		[Test]
		public void Insert_creates_and_adds_a_new_entity_to_clientId_bound_store_when_not_empty()
		{
			var newRow = Substitute.For<IDbDataRow>();
			var table = Substitute.For<IDbTable>();
			table.CreateDataRow().Returns(newRow);
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable("justDoIt_TestEntity").Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string), "justDoIt"));
			var entity = new TestEntity{Id = 911, Name = "my entry!" };

			entityStore.Insert(entity);

			CollectionAssert.IsNotEmpty(entityStore);
			newRow.Received().Update("Name", "my entry!");
			newRow.Received().Update("Id", 911);
		}

		[Test]
		public void Delete_when_removing_deletes_entity_itself_and_removes_it_from_store()
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
			db.GetTable(Arg.Any<string>()).Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string), string.Empty));
			entityStore.Reload();

			entityStore.Delete(entityStore.First());

			data.Received().Delete();
			CollectionAssert.IsEmpty(entityStore);
		}
		
		[Test]
		public void Delete_removes_entity_from_clientId_bound_db_and_from_store()
		{
			var data = Substitute.For<IDbDataRow>();
			data.Data.Returns(new Dictionary<string, object>
			{
				{"EntityID", 1}, 
				{"Name", "lastone"},
				{"Id", -1}
			});

			var table = Substitute.For<IDbTable>();
			table.DataRows.Returns(new Dictionary<int, IDbDataRow> { { 0, data } });
			var db = Container.SubstituteFor<IDatabase>();
			db.GetTable("noMoreIdeas_TestEntity").Returns(table);
			Container.Provide<IEntityDbConverter<TestEntity>>(new ReflectionEntityDbConverter<TestEntity>());
			
			IEntityStore<TestEntity> entityStore = Container.Resolve<EntityStore<TestEntity>>(new TypedParameter(typeof(string), "noMoreIdeas"));
			entityStore.Reload();

			entityStore.Delete(entityStore.First());

			data.Received().Delete();
			CollectionAssert.IsEmpty(entityStore);
		}

		[DataServiceKey("Id")]
		public class TestEntity
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}
