using System;
using ErpServices.Database;
using ErpServices.Services;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
    [TestFixture]
    public class LookupCollectionTests
    {
        [Test(Description = "Lookup collections are generic and read-only for all users and therefore " +
                            "client authentication is not needed")]
        public void ClientId_returns_empty_for_LookupCollections()
        {
            var requestClientId = Substitute.For<Func<string>>();

            var lookupCollection = new TestsErpLookupServiceMethodBaseImpl();
            lookupCollection.GetCurrentClientId = requestClientId;
            var clientId = lookupCollection.GetErpSericeMethodBaseClientId();
			
            Assert.IsEmpty(clientId);
            requestClientId.DidNotReceiveWithAnyArgs().Invoke();
        }

        [Test]
        public void LookupCollections_are_readOnly()
        {
            var testEntity = new EntityStoreTests.TestEntity() { Id = 1, Name = "Malte" };
            var lookupCollection = new LookupCollection<EntityStoreTests.TestEntity>(null);

            Assert.Throws<Exception>(() => lookupCollection.Create(testEntity));
            Assert.Throws<Exception>(() => lookupCollection.Update(testEntity));
            Assert.Throws<Exception>(() => lookupCollection.Delete(testEntity));
        }
        
        class TestsErpLookupServiceMethodBaseImpl : LookupCollection<EntityStoreTests.TestEntity>
        {
            public TestsErpLookupServiceMethodBaseImpl() : base(Substitute.For<IEntityStores>())
            {
            }

            public string GetErpSericeMethodBaseClientId()
            {
                return ClientId;
            }
        }
    }
}