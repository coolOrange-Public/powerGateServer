using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.Link;

namespace VaultServices.Tests.Entities.Base
{
    [TestFixture]
    public class EntityOperationBaseTests : ContainerBasedTests
    {
        [Test]
        public void Test_Query_QueryOperationsCanExecuteFalse_ReturnsEmptyList()
        {
            var queryOperation = Substitute.For<IQueryOperation<EntityTest>>();
            queryOperation.CanExecute(Arg.Any<IExpression<EntityTest>>()).Returns(false);
            
            var entityOperation = Container.Resolve<EntityTestService>();
            entityOperation.QueryOperations = queryOperation;
            var result = entityOperation.Query(Substitute.For<IExpression<EntityTest>>());

            queryOperation.Received().CanExecute(Arg.Any<IExpression<EntityTest>>());
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Test_Query_QueryOperationsCanExecuteTrue_ReturnsTwoEntities()
        {
            var queryOperation = Substitute.For<IQueryOperation<EntityTest>>();
            queryOperation.CanExecute(Arg.Any<IExpression<EntityTest>>()).Returns(true);
            queryOperation.Execute().Returns(new[] { new EntityTest { Id = 31 }, new EntityTest { Id = 69 } });
            
            var entityOperation = Container.Resolve<EntityTestService>();
            entityOperation.QueryOperations = queryOperation;

            var result = entityOperation.Query(Substitute.For<IExpression<EntityTest>>());
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Test_Query_QueryOperationsIsNull_ReturnsEmptyList()
        {
            var entityOperation = Container.Resolve<EntityTestService>();
            var result = entityOperation.Query(Substitute.For<IExpression<EntityTest>>());
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Test_Query_QueryOperationsWhichReturnsOnExecuteSameEntities_ReturnsListUniqueOnId()
        {
            var queryOperation = Substitute.For<IQueryOperation<EntityTest>>();
            queryOperation.CanExecute(Arg.Any<IExpression<EntityTest>>()).Returns(true);
            queryOperation.Execute().Returns(new[]
            {
                new EntityTest {Id = 31},
                new EntityTest {Id = 69},
                new EntityTest {Id = 31},
                new EntityTest {Id = 69},
                new EntityTest {Id = 69}
            });
            
            var entityOperation = Container.Resolve<EntityTestService>();
            entityOperation.QueryOperations = queryOperation;
            var result = entityOperation.Query(Substitute.For<IExpression<EntityTest>>()).ToList();

            Assert.AreEqual(2, result.Count());
            CollectionAssert.Contains(result.Select(test => test.Id), 31);
            CollectionAssert.Contains(result.Select(test => test.Id), 69);
        }
    }

    public class EntityTestService : EntityOperationBase<EntityTest>
    {
        public override void Update(EntityTest entity)
        {
            throw new NotImplementedException();
        }

        public override void Create(EntityTest entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(EntityTest entity)
        {
            throw new NotImplementedException();
        }
    }
	[DataServiceKey("Id")]
    public class EntityTest : IBaseObject
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<VaultServices.Entities.Property.Property> Properties { get; set; }
        public IEnumerable<VaultServices.Entities.Link.Link> Children { get; set; }
        public IEnumerable<VaultServices.Entities.Link.Link> Parents { get; set; }
    }
}