using System;
using System.Collections.Generic;
using System.Linq;
using ErpServices.Converters;
using ErpServices.Database;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class ExtendedReflectionEntityDbConverterTests : ContainerBaseTest
	{
		[Test]
		public void ConvertFrom_converting_a_type_with_DateTime_property_converts_the_string_value_to_correct_DateTime()
		{
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass4>>();

			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "2014-01-01 00:00:00.000" }
			});

			Assert.AreEqual(new DateTime(2014,01,01),entity.SomeProp1);
		}
		
		[Test]
		public void ConvertTo_converting_a_Type_wih_propertyOfOtherClass_returnsThePrimaryKey_as_data_value_for_complexProperty()
		{
			var testInstance = new TestClass
			{
				SomeProp1 = "This is a text",
				SomeProp2 = new TestClass2
				{
					SomeProp1 = "SomeData",
					SomeProp2 = 666
				}
			};
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			testClass2Store.Insert(testInstance.SomeProp2).Returns(999);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();
			var data = converter.ConvertTo(testInstance);

			CollectionAssert.AreEqual(new[] { "SomeProp1", "SomeProp2" },data.Keys);
			CollectionAssert.AreEqual(new object[] { "This is a text", 999 }, data.Values);
		}

		[Test]
		public void ConvertFrom_Data_with_referenceId_to_other_entityType_returns_instance_where_complexProperty_is_filled_withEntityWhere_primaryId_is_the_refId()
		{
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			var complexPropInstance = new TestClass2();
			testClass2Store.Find(999).Returns(complexPropInstance);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();
			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "This is a text" },
				{ "SomeProp2", "999" }
			});

			Assert.AreEqual("This is a text", entity.SomeProp1);
			Assert.AreSame(complexPropInstance, entity.SomeProp2);
		}

		[Test]
		public void ConvertTo_Converting_a_instance_with_empty_ComplexTypeProperty_returns_data_where_complexProperty_is_null()
		{
			var testInstance = new TestClass
			{
				SomeProp1 = "This is a text"
			};
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();
			var data = converter.ConvertTo(testInstance);

			CollectionAssert.AreEqual(new[] { "SomeProp1", "SomeProp2" }, data.Keys);
			CollectionAssert.AreEqual(new object[] { "This is a text", null }, data.Values);
		}

		[Test]
		public void ConvertFrom_data_where_refId_is_null_returns_entity_where_complexPropertyIsNull()
		{
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();

			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "This is a text" },
				{ "SomeProp2", null }
			});

			Assert.AreEqual("This is a text", entity.SomeProp1);
			Assert.IsNull(entity.SomeProp2);
		}

		[Test]
		public void ConvertTo_Converting_a_instance_with_ComplexPropertyList_returns_entity_where_property_contains_list_of_primaryKeys()
		{
			var testInstance = new TestClass3
			{
				SomeProp1 = "This is a text",
				SomeProp2 = new List<TestClass2>
				{
					new TestClass2
					{
						SomeProp1 = "Some Value",
						SomeProp2 = 666
					},
					new TestClass2
					{
						SomeProp1 = "Marlene Dietrich",
						SomeProp2 = 0
					}
				}
			};
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			testClass2Store.Insert(testInstance.SomeProp2.ElementAt(0)).Returns(999);
			testClass2Store.Insert(testInstance.SomeProp2.ElementAt(1)).Returns(888);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass3>>();
			var data = converter.ConvertTo(testInstance);

			CollectionAssert.AreEqual(new[] { "SomeProp1", "SomeProp2" }, data.Keys);
			CollectionAssert.AreEqual(new object[] { "This is a text", "999;888" }, data.Values);
		}

		[Test]
		public void ConvertFrom_data_where_refId_Is_list_of_refIds_returns_entity_where_list_propertyIs_set_with_Entities_for_each_refId()
		{
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			var complexPropInstance1 = new TestClass2();
			var complexPropInstance2 = new TestClass2();
			testClass2Store.Find(999).Returns(complexPropInstance1);
			testClass2Store.Find(888).Returns(complexPropInstance2);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass3>>();
			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "This is a text" },
				{ "SomeProp2", "999;888" }
			});

			CollectionAssert.AreEqual(new[] { complexPropInstance1, complexPropInstance2 }, entity.SomeProp2);
		}

		[Test]
		public void ConvertTo_Converting_a_instance_with_empty_complexPropertyList_returns_data_where_value_is_emptyString()
		{
			var testInstance = new TestClass3
			{
				SomeProp2 = Enumerable.Empty<TestClass2>()
			};
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass3>>();
			var data = converter.ConvertTo(testInstance);

			CollectionAssert.AreEqual(new[] { "SomeProp1", "SomeProp2" }, data.Keys);
			CollectionAssert.AreEqual(new object[] { null, "" }, data.Values);
		}

		[Test]
		public void ConvertFrom_data_where_refId_Is_EmptyList_of_refIds_returns_entity_where_list_propertyIs_EmptyButNotNull()
		{
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass3>>();

			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "This is a text" },
				{ "SomeProp2", "" }
			});

			CollectionAssert.IsEmpty(entity.SomeProp2);
		}
		
		[Test]
		public void ConvertTo_uses_entityStore_bound_to_clientId_for_refIds_when_client_authenticated()
		{
			var getClientId = Substitute.For<Func<string>>();
			getClientId.Invoke().ReturnsForAnyArgs("somevalidClientId");
			var testInstance = new TestClass
			{
				SomeProp1 = "This is a text",
				SomeProp2 = new TestClass2
				{
					SomeProp1 = "SomeData",
					SomeProp2 = 666
				}
			};
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			testClass2Store.Insert(testInstance.SomeProp2).Returns(999);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();
			converter.GetCurrentClientId = getClientId;
			var data = converter.ConvertTo(testInstance);

			CollectionAssert.AreEqual(new[] { "SomeProp1", "SomeProp2" }, data.Keys);
			CollectionAssert.AreEqual(new object[] { "This is a text", 999 }, data.Values);
			stores.Received(1).ResolveFor<TestClass2>(Arg.Is("somevalidClientId"));
		}

		[Test]
		public void ConvertFrom_uses_entityStore_bound_to_clientId_for_refIds_when_client_authenticated()
		{
			var getClientId = Substitute.For<Func<string>>();
			getClientId.Invoke().Returns("yetanotherClientId");
			var stores = Container.SubstituteFor<IEntityStores>();
			var testClass2Store = Substitute.For<IEntityStore<TestClass2>>();
			var complexPropInstance = new TestClass2();
			testClass2Store.Find(999).Returns(complexPropInstance);
			stores.ResolveFor<TestClass2>(Arg.Any<string>()).Returns(testClass2Store);
			
			var converter = Container.Resolve<ExtendedReflectionEntityDbConverter<TestClass>>();
			converter.GetCurrentClientId = getClientId;
			var entity = converter.ConvertFrom(new Dictionary<string, object>
			{
				{ "SomeProp1", "This is a text" },
				{ "SomeProp2", "999" }
			});

			Assert.AreEqual("This is a text", entity.SomeProp1);
			Assert.AreSame(complexPropInstance, entity.SomeProp2);
			stores.Received(1).ResolveFor<TestClass2>(Arg.Is("yetanotherClientId"));
		}

		[Test(Description = "Lookups are generic and read-only for all users and therefore " +
		                    "client authentication is not needed")]
		public void ClientId_returns_empty_for_Lookup_entities()
		{
			var getClientId = Substitute.For<Func<string>>();
			getClientId.Invoke().Returns("DaiDai");

			var converter = new ExtendedReflectionEntityDbConverter<TestLookupClass>(null);
			converter.GetCurrentClientId = getClientId;
			
			Assert.IsEmpty(converter.ClientId);
			getClientId.DidNotReceiveWithAnyArgs().Invoke();
		}

		public class TestClass
		{
			public string SomeProp1 { get; set; }
			public TestClass2 SomeProp2 { get; set; }
		}

		public class TestClass2
		{
			public string SomeProp1 { get; set; }
			public int SomeProp2 { get; set; }
		}

		public class TestClass3
		{
			public string SomeProp1 { get; set; }
			public IEnumerable<TestClass2> SomeProp2 { get; set; }
		}

		public class TestClass4
		{
			public DateTime SomeProp1 { get; set; }
		}

		public class TestLookupClass
		{
		}
	}
}
