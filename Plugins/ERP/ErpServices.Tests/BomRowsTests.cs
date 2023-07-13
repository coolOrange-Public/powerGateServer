using System;
using System.Collections.Generic;
using System.Linq;
using ErpServices.Database;
using ErpServices.Services.Boms;
using ErpServices.Services.Entities;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class BomRowsTests : ContainerBaseTest
	{
		[Test]
		public void Query_returns_empty_when_no_bomRows_in_entityStore()
		{
			var bomRows = Container.Resolve<BomRows>();
			
			Assert.IsEmpty(bomRows.Query(null));
		}

		[Test]
		public void Query_returns_all_bomRows_in_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(Arg.Any<string>()).Returns(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5"}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			
			var bomRows = Container.Resolve<BomRows>();
			var result = bomRows.Query(null).ToList();
			
			Assert.AreEqual(3, result.Count());
			Assert.IsTrue(bomRowsInStore.Select(bomRow => bomRow.ChildNumber+bomRow.ParentNumber).SequenceEqual(result.Select(row => row.ChildNumber+row.ParentNumber)));
		}

		[Test]
		public void Query_returns_bomRows_with_Item_navigation_property_resolved_by_ChildNumber()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>("myClientId1").Returns(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5"}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("myClientId1").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() { Number = "not the right One" }, new Item() { Number = "10" } };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());

			var bomRows = Container.Resolve<BomRows>();
			bomRows.GetCurrentClientId = () => "myClientId1";
			var result = bomRows.Query(null).ToList();

			Assert.AreEqual(3, result.Count());
			Assert.AreSame(itemsInStore.First(i => i.Number == "10"), result.First(row => row.ChildNumber == "10" && row.ParentNumber == "5").Item);
			Assert.AreSame(itemsInStore.First(i => i.Number == "10"), result.First(row => row.ChildNumber == "10" && row.ParentNumber == "1").Item);
			Assert.IsNull(result.First(row => row.ChildNumber == "11").Item);
		}

		[Test]
		public void Update_replaces_bomRows_in_store_with_same_ParentNumber_and_ChildNumber()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5", Quantity = 6.66m}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.WhenForAnyArgs(info => info.Delete(null)).Do(info =>
			{
				var row = info.Arg<BomRow>();
				bomRowsInStore.Remove(bomRowsInStore.First(r => r.ChildNumber == row.ChildNumber && r.ParentNumber == row.ParentNumber));
			});
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());

			var bomRows = Container.Resolve<BomRows>();
			var bomRowUpdated = new BomRow(){ChildNumber = "11", ParentNumber = "5", Quantity = 1000m};
			bomRows.Update(bomRowUpdated);
			
			bomRowStore.Received(1).Delete(Arg.Is<BomRow>(b => b.ChildNumber == "11" && b.ParentNumber == "5" && b.Quantity == 6.66m));
			bomRowStore.Received(1).Insert(bomRowUpdated);
		}

		[Test]
		public void Update_throws_when_bomRow_with_ParentNumber_and_ChildNumber_are_not_in_store()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5", Quantity = 6.66m}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			
			var bomRows = Container.Resolve<BomRows>();
			var bomRowUpdated = new BomRow(){ChildNumber = "1000", ParentNumber = "1000", Quantity = 10m};
			
			var exception = Assert.Throws<Exception>(() => bomRows.Update(bomRowUpdated));
			Assert.AreEqual("A BomRow with key: [ParentNumber=1000;ChildNumber=1000] was not found!", exception.Message);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			bomRowStore.DidNotReceiveWithAnyArgs().Delete(null);
		}

		[Test]
		public void Delete_removes_bomRow_with_same_ParentNumber_and_ChildNumber_from_store()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5", Quantity = 6.66m}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			
			var bomRows = Container.Resolve<BomRows>();
			bomRows.Delete(new BomRow(){ChildNumber = "11", ParentNumber = "5"});
			
			bomRowStore.Received(1).Delete(Arg.Is<BomRow>(b => b.ChildNumber == "11" && b.ParentNumber == "5"));
		}

		[Test]
		public void Delete_throw_when_no_bomRow_with_same_ParentNumber_and_ChildNumber_exists_in_store()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			
			var bomRows = Container.Resolve<BomRows>();
			
			var exception = Assert.Throws<Exception>(() => bomRows.Delete(new BomRow(){ChildNumber = "11", ParentNumber = "5"}));
			Assert.AreEqual("A BomRow with key: [ParentNumber=5;ChildNumber=11] was not found!", exception.Message);
			bomRowStore.DidNotReceiveWithAnyArgs().Delete(null);
		}

		[Test]
		public void Create_adds_passed_bomRow_to_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);

			var bomRows = Container.Resolve<BomRows>();
			bomRows.Create(new BomRow(){ChildNumber = "123", ParentNumber = "456"});

			bomRowStore.Received(1).Insert(Arg.Is<BomRow>(b => b.ChildNumber == "123" && b.ParentNumber == "456"));
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}

		[Test]
		public void Create_also_creates_Items_passed_with_bomRow()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);

			var bomRows = Container.Resolve<BomRows>();
			bomRows.Create(new BomRow(){ChildNumber = "1000", ParentNumber = "5678", Item = new Item(){Number = "1000"}});

			bomRowStore.Insert(Arg.Is<BomRow>(b => b.ChildNumber == "1000" && b.ParentNumber == "5678"));
			itemStore.Insert(Arg.Is<Item>(i => i.Number == "1000"));
		}

		[Test]
		public void Create_throws_when_bomRow_with_same_ParentNumber_and_ChildNumber_already_exists_in_entityStore_and_does_not_create_bomRow()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "11", ParentNumber = "5", Quantity = 6.66m}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			

			var bomRows = Container.Resolve<BomRows>();
			var exception = Assert.Throws<Exception>(() => bomRows.Create(new BomRow(){ChildNumber = "11", ParentNumber = "5", Quantity = 1234m, Item = new Item(){Number = "idonotexist"}}));
			Assert.AreEqual("A BomRow with key: [ParentNumber=5;ChildNumber=11] already exists!", exception.Message);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}

		[Test]
		public void Create_throws_when_bomRow_with_already_existing_Item_is_passed()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var bomRowsInStore = new List<BomRow>() { new BomRow() { ChildNumber = "10", ParentNumber = "5"}, new BomRow() {ChildNumber = "10", ParentNumber = "1"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			var itemStore = Substitute.For<IEntityStore<Item>>();
			var itemsInStore = new List<Item>() { new Item() { Number = "1000", Description = "Already existing item" } };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);

			var bomRows = Container.Resolve<BomRows>();
			var exception = Assert.Throws<Exception>(() => bomRows.Create(new BomRow(){ChildNumber = "1000", ParentNumber = "5678", Item = new Item(){Number = "1000"}}));
			Assert.AreEqual("A Item with key: [Number=1000] already exists!", exception.Message);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}
	}
}