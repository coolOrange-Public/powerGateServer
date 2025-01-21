using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SapServices.Converters;
using SapServices.Database;

namespace SapServices.Tests
{
	[TestFixture]
	public class EntityStoresTests : ContainerBaseTest
	{

		[Test]
		public void AddStore_registers_a_new_default_store()
		{
			Container.SubstituteFor<IDatabase>()
				.Tables.Returns(new Dictionary<string, IDbTable> { { "TestEntity", Substitute.For<IDbTable>() } });
			var stores = Container.Resolve<EntityStores>();
			var stringStore = stores.AddStoreFor<string>();
			Assert.IsInstanceOf<IEntityStore<string>>(stringStore);
			Assert.IsInstanceOf<ExtendedReflectionEntityDbConverter<string>>(stringStore.Converter);
		}

		[Test]
		public void AddStoreWithConverter_registers_a_new_store_with_the_custom_Converter()
		{
			Container.SubstituteFor<IDatabase>()
				.Tables.Returns(new Dictionary<string, IDbTable> { { "TestEntity", Substitute.For<IDbTable>() } });
			var customConverter = Substitute.For<IEntityDbConverter<EntityStoreTests.TestEntity>>();
			var stores = Container.Resolve<EntityStores>();

			var testStore = stores.AddStoreFor(customConverter);

			Assert.IsInstanceOf<IEntityStore<EntityStoreTests.TestEntity>>(testStore);
			Assert.AreSame(customConverter, testStore.Converter);
		}

		[Test]
		public void ResolveFor_returns_the_registered_store_for_that_type()
		{
			Container.SubstituteFor<IDatabase>()
				.Tables.Returns(new Dictionary<string, IDbTable> { { "TestEntity", Substitute.For<IDbTable>() } });
			var stores = Container.Resolve<EntityStores>();
			var testStore = stores.AddStoreFor<EntityStoreTests.TestEntity>();

			var testStoreResult = stores.ResolveFor<EntityStoreTests.TestEntity>();
			Assert.AreSame(testStore,testStoreResult);
		}
	}
}
