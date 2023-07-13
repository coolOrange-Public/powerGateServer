using System;
using System.Collections.Generic;
using System.Linq;
using ErpServices.Database;
using ErpServices.Services.Entities;
using ErpServices.Services.Items;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class ItemsTests : ContainerBaseTest
	{
		[Test]
		public void Query_returns_all_Items_from_entityStore_with_same_clientId()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId111").Returns(itemStore);
			var itemsInStore = new[] { new Item() {Number = "Mat. 1"}, new Item(){Number = "Mat. 2"}, new Item() {Number = "Mat. 3"}}.ToList();
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());

			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId111";

			var result = items.Query(null).ToList();
			Assert.AreEqual(3, result.Count());
			CollectionAssert.AreEquivalent(itemsInStore, result);
		}

		[Test]
		public void Query_returns_empty_when_no_items_exist()
		{
			Container.SubstituteFor<IEntityStores>();

			var items = Container.Resolve<Items>();
			Assert.IsEmpty(items.Query(null));
		}

		[Test]
		public void Update_replaces_items_in_store_with_same_Number_with_passed_item()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId222").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "my item number"}, new Item() {Number = "my item number 2"}, new Item() {Number = "my item number 3"} };
			itemStore.WhenForAnyArgs(info => info.Delete(null)).Do(info =>
			{
				var item = info.Arg<Item>();
				itemsInStore.Remove(itemsInStore.First(i => i.Number == item.Number));
			});
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			var entityNewValue = new Item(){Number = "my item number 2"};

			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId222";
			items.Update(entityNewValue);

			itemStore.Received(1).Delete(Arg.Is<Item>(i => i.Number == "my item number 2"));
			itemStore.Received(1).Insert(entityNewValue);
		}

		[Test]
		public void Update_throws_when_item_does_not_exist()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId333").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "my item number"}, new Item() {Number = "my item number 2"}, new Item() {Number = "my item number 3"} };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			
			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId333";
			
			var exception = Assert.Throws<Exception>(() => items.Update(new Item(){Number = "jo obr mi gibs jo a net"}));
			Assert.AreEqual("A Item with key: [Number=jo obr mi gibs jo a net] was not found!", exception.Message);
		}

		[Test]
		public void Delete_removes_item_with_passed_number_from_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId444").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "my item number"}, new Item() {Number = "to delete"}, new Item() {Number = "my item number 3"} };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());

			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId444";
			items.Delete(new Item(){Number = "to delete"});
			
			itemStore.Received(1).Delete(Arg.Is<Item>(i => i.Number == "to delete"));
		}

		[Test]
		public void Delete_throws_when_item_does_not_exist()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId555").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "my item number"}, new Item() {Number = "my item number 2"}, new Item() {Number = "my item number 3"} };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			
			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId555";

			var exception = Assert.Throws<Exception>(() => items.Delete(new Item(){Number = "MeNoExisty"}));
			Assert.AreEqual("A Item with key: [Number=MeNoExisty] was not found!", exception.Message);
		}

		[Test]
		public void Create_adds_passed_item_to_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId666").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "my item number"}, new Item() {Number = "my item number 2"}, new Item() {Number = "my item number 3"} };
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());

			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId666";
			items.Delete(new Item(){Number = "my item number 2"});
			
			itemStore.Received(1).Delete(Arg.Is<Item>(i => i.Number =="my item number 2"));
		}

		[Test]
		public void Create_throws_when_Item_with_key_already_exists()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId777").Returns(itemStore);
			var itemsInStore = new List<Item>() { new Item() {Number = "already existing item"}};
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());

			var items = Container.Resolve<Items>();
			items.GetCurrentClientId = () => "clientId777";
			var exception = Assert.Throws<Exception>(() => items.Create(new Item() { Number = "already existing item", Description = "some description", Material = "some material" }));
			Assert.AreEqual("A Item with key: [Number=already existing item] already exists!", exception.Message);
		}
	}
}