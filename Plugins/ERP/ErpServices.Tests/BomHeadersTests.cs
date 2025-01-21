using System;
using System.Collections;
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
	public class BomHeadersTests : ContainerBaseTest
	{
		[Test]
		public void Query_returns_empty_when_no_bomHeaders_in_entityStore()
		{
			var bomHeaders = Container.Resolve<BomHeaders>();
			
			Assert.IsEmpty(bomHeaders.Query(null));
		}

		[Test]
		public void Query_returns_all_bomHeaders_in_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			var result = bomHeaders.Query(null).ToList();

			Assert.AreEqual(3, result.Count());
			Assert.AreEqual("Descr. 1", result.First(header => header.Number == "1000").Description);
			Assert.AreEqual(123.456m, result.First(header => header.Number == "1000").BaseQuantity);
			Assert.AreEqual("Descr. 2", result.First(header => header.Number == "1001").Description);
			Assert.AreEqual(456.123m, result.First(header => header.Number == "1001").BaseQuantity);
			Assert.AreEqual("Descr. 3", result.First(header => header.Number == "1010").Description);
			Assert.AreEqual(45.12m, result.First(header => header.Number == "1010").BaseQuantity);
		}

		[Test]
		public void Query_returns_bomHeaders_with_rows_and_item_navigation_properties()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>("clientId1").Returns(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){
				new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1234", Description = "Descr. 1234", BaseQuantity = 11.11m}
			};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>("clientId1").Returns(bomRowStore);
			var bomRowsInStore = new List<BomRow>()
			{
				new BomRow(){ParentNumber = "1000", ChildNumber = "Item 1", Quantity = 123.5m, Position = 1},
				new BomRow(){ParentNumber = "1001", ChildNumber = "Item 1", Quantity = 1200.5m, Position = 1},
				new BomRow(){ParentNumber = "1000", ChildNumber = "Item 2", Quantity = 1.5m, Position = 2},
				new BomRow(){ParentNumber = "1000", ChildNumber = "Item 33", Quantity = 1m, Position = 3},
				new BomRow(){ParentNumber = "9999", ChildNumber = "Item 14", Quantity = 10.5m, Position = 21},
			};
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>("clientId1").Returns(itemStore);
			var itemsInStore = new List<Item>()
			{
				new Item(){Number = "Item 1", Description = "Item 1 descr."},
				new Item(){Number = "Item 2", Description = "Item 2 descr."},
				new Item(){Number = "Item 25", Description = "Item 25 descr."}
			};
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.GetCurrentClientId = () => "clientId1";
			var result = bomHeaders.Query(null).ToList();

			Assert.AreEqual(3, result.First(header => header.Number == "1000").Children.Count());
			Assert.AreEqual(1, result.First(header => header.Number == "1001").Children.Count());
			Assert.AreEqual(0, result.First(header => header.Number == "1234").Children.Count());
			
			Assert.AreEqual(123.5m, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1000").Quantity);
			Assert.AreEqual(1, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1000").Position);
			Assert.AreEqual(1.5m, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 2" && row.ParentNumber == "1000").Quantity);
			Assert.AreEqual(2, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 2" && row.ParentNumber == "1000").Position);
			Assert.AreEqual(1m, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 33" && row.ParentNumber == "1000").Quantity);
			Assert.AreEqual(3, result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 33" && row.ParentNumber == "1000").Position);
			
			Assert.AreEqual(1200.5m, result.First(header => header.Number == "1001").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1001").Quantity);
			Assert.AreEqual(1, result.First(header => header.Number == "1001").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1001").Position);
			
			Assert.AreEqual("Item 1 descr.", result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1000").Item.Description);
			Assert.AreEqual("Item 1 descr.", result.First(header => header.Number == "1001").Children.First(row => row.ChildNumber == "Item 1" && row.ParentNumber == "1001").Item.Description);
			Assert.AreEqual("Item 2 descr.", result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 2" && row.ParentNumber == "1000").Item.Description);
			Assert.IsNull(result.First(header => header.Number == "1000").Children.First(row => row.ChildNumber == "Item 33" && row.ParentNumber == "1000").Item);
		}

		[Test]
		public void Query_retrieve_entities_from_Item_and_BomRow_entityStores_only_once()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			var bomHeadersInStore = new List<BomHeader>(){
				new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1234", Description = "Descr. 1234", BaseQuantity = 11.11m}
			};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			var bomRowsInStore = new List<BomRow>() {
				new BomRow() { ParentNumber = "1000", ChildNumber = "Item 1", Quantity = 123.5m, Position = 1 },
				new BomRow() { ParentNumber = "1001", ChildNumber = "Item 1", Quantity = 1200.5m, Position = 1 },
				new BomRow() { ParentNumber = "1000", ChildNumber = "Item 2", Quantity = 1.5m, Position = 2 }
			};
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var itemStore = Substitute.For<IEntityStore<Item>>();
			var itemsInStore = new List<Item>()
			{
				new Item(){Number = "Item 1", Description = "Item 1 descr."},
				new Item(){Number = "Item 2", Description = "Item 2 descr."}
			};
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.Query(null).ToList();
			
			bomRowStore.Received(1).GetEnumerator();
			itemStore.Received(1).GetEnumerator();
		}

		[Test]
		public void Update_replaces_bomHeader_in_entityStore_with_same_Number()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.WhenForAnyArgs(info => info.Delete(null)).Do(info =>
			{
				var header = info.Arg<BomHeader>();
				bomHeadersInStore.Remove(bomHeadersInStore.First(h => h.Number == header.Number));
			});
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.Update(new BomHeader() { Number = "1001", BaseQuantity = 23.5m, Description = "my new description"});
			
			bomHeaderStore.Received(1).Delete(Arg.Is<BomHeader>(b => b.Number == "1001"));
			bomHeaderStore.Received(1).Insert(Arg.Is<BomHeader>(b => b.Number == "1001" && b.BaseQuantity == 23.5m && b.Description == "my new description"));
		}

		[Test]
		public void Update_throws_when_no_bomHeader_with_same_Number_in_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			var exception = Assert.Throws<Exception>(()=> bomHeaders.Update(new BomHeader() { Number = "MeNoExisty", BaseQuantity = 23.5m, Description = "my new description"}));
			Assert.AreEqual("A BomHeader with key: [Number=MeNoExisty] was not found!", exception.Message);
			bomHeaderStore.DidNotReceiveWithAnyArgs().Delete(null);
			bomHeaderStore.DidNotReceiveWithAnyArgs().Insert(null);
		}

		[Test]
		public void Delete_removes_bomHeader_with_same_Number_from_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.Delete(new BomHeader(){Number = "1010", Description = "doesn't matter whats in here", BaseQuantity = 234m});
			
			bomHeaderStore.Received(1).Delete(Arg.Is<BomHeader>(b => b.Number == "1010"));
		}

		[Test]
		public void Delete_throws_when_no_bomHeader_with_same_Number_in_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			var exception = Assert.Throws<Exception>(() => bomHeaders.Delete(new BomHeader(){Number = "neither do I existy", Description = "doesn't matter whats in here", BaseQuantity = 234m}));
			Assert.AreEqual("A BomHeader with key: [Number=neither do I existy] was not found!", exception.Message);
			bomHeaderStore.DidNotReceiveWithAnyArgs().Delete(null);
		}

		[Test]
		public void Create_adds_passed_bomHeader_to_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.Create(new BomHeader(){Number = "bomHeader 1", Description = "Descr h1", BaseQuantity = 23m});

			bomHeaderStore.Received(1).Insert(Arg.Is<BomHeader>(b => b.Number == "bomHeader 1" && b.Description == "Descr h1" && b.BaseQuantity == 23m));
		}

		[Test]
		public void Create_also_creates_passed_bomRows_and_their_items()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			bomHeaders.Create(new BomHeader()
			{
				Number = "10",
				Description = "testme",
				BaseQuantity = 123.56m,
				Children = new []
				{
					new BomRow()
					{
						ParentNumber = "10",
						ChildNumber = "Item 1",
						Position = 1,
						Quantity = 2.65m,
						Item = new Item()
						{
							Number = "Item 1",
							Material = "some mat.1",
							MakeBuy = false
						}
					},
					new BomRow()
					{
						ParentNumber = "10",
						ChildNumber = "Item 2",
						Position = 2,
						Quantity = 2m,
						Item = new Item()
						{
							Number = "Item 2",
							Material = "some mat.2",
							MakeBuy = true
						}
					}
				}
			});

			bomHeaderStore.Received(1).Insert(Arg.Is<BomHeader>(b => b.Number == "10" && b.Description == "testme" && b.BaseQuantity == 123.56m));
			bomRowStore.Received(1).Insert(Arg.Is<BomRow>(b => b.ParentNumber == "10" && b.ChildNumber == "Item 1" && b.Position == 1 && b.Quantity == 2.65m));
			bomRowStore.Received(1).Insert(Arg.Is<BomRow>(b => b.ParentNumber == "10" && b.ChildNumber == "Item 2" && b.Position == 2 && b.Quantity == 2m));
			itemStore.Received(1).Insert(Arg.Is<Item>(i => i.Number == "Item 1" && i.Material == "some mat.1" && i.MakeBuy == false));
			itemStore.Received(1).Insert(Arg.Is<Item>(i => i.Number == "Item 2" && i.Material == "some mat.2" && i.MakeBuy == true));
		}

		[Test]
		public void Create_throws_when_bomHeader_with_same_Number_already_exists_in_entityStore()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomHeadersInStore = new List<BomHeader>(){new BomHeader(){Number = "1000", Description = "Descr. 1", BaseQuantity = 123.456m}, 
				new BomHeader(){Number = "1001", Description = "Descr. 2", BaseQuantity = 456.123m},
				new BomHeader(){Number = "1010", Description = "Descr. 3", BaseQuantity = 45.12m}};
			bomHeaderStore.GetEnumerator().Returns(info => bomHeadersInStore.GetEnumerator());
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			var exception = Assert.Throws<Exception>(() => bomHeaders.Create(new BomHeader()
			{
				Number = "1001", 
				Description = "Descr h1", 
				BaseQuantity = 23m,
				Children = new []{new BomRow()
				{
					ParentNumber = "1001",
					ChildNumber = "doesntexist",
					Item = new Item() { Number = "doesntexist" }
				}}
				
			}));
			Assert.AreEqual("A BomHeader with key: [Number=1001] already exists!", exception.Message);

			bomHeaderStore.DidNotReceiveWithAnyArgs().Insert(null);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}

		[Test]
		public void Create_throws_and_does_not_insert_any_entities_when_passed_bomRow_already_exists()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			var bomRowsInStore = new List<BomRow>() {new BomRow() {ParentNumber = "1", ChildNumber = "alreadyexists"} };
			bomRowStore.GetEnumerator().Returns(info => bomRowsInStore.GetEnumerator());
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var itemStore = Substitute.For<IEntityStore<Item>>();
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			
			var exceptionBomRow = Assert.Throws<Exception>(() => bomHeaders.Create(new BomHeader()
			{
				Number = "1",
				Children = new []
				{
					new BomRow()
					{
						ParentNumber = "1",
						ChildNumber = "doesnotexist"
					},
					new BomRow()
					{
						ParentNumber = "1",
						ChildNumber = "alreadyexists"
					}
				}
			}));
			Assert.AreEqual("A BomRow with key: [ParentNumber=1;ChildNumber=alreadyexists] already exists!", exceptionBomRow.Message);
			bomHeaderStore.DidNotReceiveWithAnyArgs().Insert(null);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}
		
		[Test]
		public void Create_throws_and_does_not_insert_any_entities_when_passed_Item_already_exists()
		{
			var entityStores = Container.SubstituteFor<IEntityStores>();
			var bomHeaderStore = Substitute.For<IEntityStore<BomHeader>>();
			entityStores.ResolveFor<BomHeader>(null).ReturnsForAnyArgs(bomHeaderStore);
			var bomRowStore = Substitute.For<IEntityStore<BomRow>>();
			entityStores.ResolveFor<BomRow>(null).ReturnsForAnyArgs(bomRowStore);
			var itemStore = Substitute.For<IEntityStore<Item>>();
			var itemsInStore = new List<Item>() { new Item(){Number = "alreadyexists"}};
			itemStore.GetEnumerator().Returns(info => itemsInStore.GetEnumerator());
			entityStores.ResolveFor<Item>(null).ReturnsForAnyArgs(itemStore);
			
			var bomHeaders = Container.Resolve<BomHeaders>();
			
			var exceptionBomRow = Assert.Throws<Exception>(() => bomHeaders.Create(new BomHeader()
			{
				Number = "1",
				Children = new []
				{
					new BomRow()
					{
						ParentNumber = "1",
						ChildNumber = "doesnotexist",
						Item = new Item(){Number = "doesnotexist"}
					},
					new BomRow()
					{
						ParentNumber = "1",
						ChildNumber = "alreadyexists",
						Item = new Item(){Number = "alreadyexists"}
					}
				}
			}));
			Assert.AreEqual("A Item with key: [Number=alreadyexists] already exists!", exceptionBomRow.Message);
			bomHeaderStore.DidNotReceiveWithAnyArgs().Insert(null);
			bomRowStore.DidNotReceiveWithAnyArgs().Insert(null);
			itemStore.DidNotReceiveWithAnyArgs().Insert(null);
		}
	}
}