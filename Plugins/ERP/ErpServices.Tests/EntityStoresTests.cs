using ErpServices.Converters;
using ErpServices.Database;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class EntityStoresTests : ContainerBaseTest
	{
		[Test]
		public void AddStore_registers_a_new_default_store()
		{
			var stores = Container.Resolve<EntityStores>();
			
			stores.AddStoreFor<string>();
			var stringStore = stores.ResolveFor<string>(string.Empty);

			Assert.IsInstanceOf<IEntityStore<string>>(stringStore);
			Assert.IsInstanceOf<ReflectionEntityDbConverter<string>>(stringStore.Converter);
		}

		[Test]
		public void AddStoreWithConverter_registers_a_new_store_with_the_custom_Converter()
		{
			var customConverter = Substitute.For<IEntityDbConverter<EntityStoreTests.TestEntity>>();
			
			var stores = Container.Resolve<EntityStores>();
			stores.AddStoreFor(customConverter);
			var testStore = stores.ResolveFor<EntityStoreTests.TestEntity>(string.Empty);
			
			Assert.IsInstanceOf<IEntityStore<EntityStoreTests.TestEntity>>(testStore);
			Assert.AreSame(customConverter, testStore.Converter);
		}

		[Test]
		public void ResolveFor_returns_default_registered_store_for_that_type_when_empty_clientId_is_passed()
		{
			var stores = Container.Resolve<EntityStores>();
			stores.AddStoreFor<EntityStoreTests.TestEntity>();

			var testStoreResult = stores.ResolveFor<EntityStoreTests.TestEntity>(string.Empty);
			var testStore = stores.ResolveFor<EntityStoreTests.TestEntity>(string.Empty);
			Assert.AreSame(testStore,testStoreResult);
		}
		
		[Test]
		public void ResolveFor_returns_same_store_for_that_type_when_for_same_clientId()
		{
			var stores = Container.Resolve<EntityStores>();
			stores.AddStoreFor<EntityStoreTests.TestEntity>();

			stores.ResolveFor<EntityStoreTests.TestEntity>("client0");
			var testStore1 = stores.ResolveFor<EntityStoreTests.TestEntity>("Client1");
			var testStore2 = stores.ResolveFor<EntityStoreTests.TestEntity>("Client1");
			Assert.AreSame(testStore1,testStore2);
		}

		[Test]
		public void ResolveFor_returns_customer_specific_stores_when_id_is_passed()
		{
			var stores = Container.Resolve<EntityStores>();
			stores.AddStoreFor<EntityStoreTests.TestEntity>();

			var testStoreResult = stores.ResolveFor<EntityStoreTests.TestEntity>(string.Empty);
			var testStore = stores.ResolveFor<EntityStoreTests.TestEntity>("clientId1");
			Assert.AreNotSame(testStore,testStoreResult);
		}
	}
}
