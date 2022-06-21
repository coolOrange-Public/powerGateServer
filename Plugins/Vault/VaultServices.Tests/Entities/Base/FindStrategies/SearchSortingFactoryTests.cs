using Autodesk.Connectivity.WebServices;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base.FindStrategies;

namespace VaultServices.Tests.Entities.Base.FindStrategies
{
	[TestFixture]
	public class SearchSortingFactoryTests : ContainerBasedTests
	{
		[Test]
		public void Crates_sort_for_injected_property_definition()
		{
			var sorting = Container.Resolve<SearchSortingFactory>();
			var sort = sorting.Create(new PropDef { Id = 666 }, Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>());
			Assert.AreEqual(666,sort.PropDefId);
		}

		[Test]
		public void Crating_simple_sort_for_ordering_token_is_ascending()
		{
			var orderToken = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			orderToken.Method.Returns(OrderingMethod.OrderBy);
			var sorting = Container.Resolve<SearchSortingFactory>();
			var sort = sorting.Create(new PropDef(), orderToken);
			Assert.IsTrue(sort.SortAsc);
		}

		[Test]
		public void Crating_thenBy_sort_for_ordering_token_is_ascending()
		{
			var orderToken = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			orderToken.Method.Returns(OrderingMethod.ThenBy);
			var sorting = Container.Resolve<SearchSortingFactory>();
			var sort = sorting.Create(new PropDef(), orderToken);
			Assert.IsTrue(sort.SortAsc);
		}

		[Test]
		public void Crating_simple_sort_for_ordering_token_is_descending()
		{
			var orderToken = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			orderToken.Method.Returns(OrderingMethod.OrderByDescending);
			var sorting = Container.Resolve<SearchSortingFactory>();
			var sort = sorting.Create(new PropDef(), orderToken);
			Assert.IsFalse(sort.SortAsc);
		}

		[Test]
		public void Crating_thenBy_sort_for_ordering_token_is_descending()
		{
			var orderToken = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			orderToken.Method.Returns(OrderingMethod.ThenByDescending);
			var sorting = Container.Resolve<SearchSortingFactory>();
			var sort = sorting.Create(new PropDef(), orderToken);
			Assert.IsFalse(sort.SortAsc);
		}
	}
}