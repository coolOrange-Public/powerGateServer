using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.File.FindStrategies;


namespace VaultServices.Tests.Entities.File.FindStrategies
{
	[TestFixture]
	public class FindFilesTests : ContainerBasedTests
	{
		[Test]
		public void CanExecute_when_atleast_one_strategy_isValid_returnsTrue()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy.CanExecute(expression).Returns(true);
			var findStrategy2 = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy2.CanExecute(expression).Returns(false);

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] {findStrategy, findStrategy2});
			var findFiles = Container.Resolve<FindFiles>();
			Assert.IsTrue(findFiles.CanExecute(expression));
		}

		[Test]
		public void CanExecute_when_all_strategies_areInvalid_returnsFalse()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy.CanExecute(expression).Returns(false);

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] { findStrategy });
			var findFiles = Container.Resolve<FindFiles>();
			Assert.IsFalse(findFiles.CanExecute(expression));
		}

		[Test]
		public void Execute_when_no_strategy_isValid_ReturnsEmptyList()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy.CanExecute(expression).Returns(false);

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] { findStrategy });
			var findFiles = Container.Resolve<FindFiles>();

			Assert.IsFalse(findFiles.CanExecute(expression));
			CollectionAssert.IsEmpty(findFiles.Execute());
		}

		[Test]
		public void Test_Execute_WithOneStrategy_ReturnsTheFilesForThatStrategy()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			var file = new VaultServices.Entities.File.File { Id = 31 };
			findStrategy.CanExecute(expression).Returns(true);
			findStrategy.Execute().Returns(new[] { file });

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] { findStrategy });
			var findFiles = Container.Resolve<FindFiles>();

			Assert.IsTrue(findFiles.CanExecute(expression));
			Assert.AreEqual(new[] { file }, findFiles.Execute());
		}

		[Test]
		public void Execute_with_multiple_strategies_returns_the_files_from_both_strategies()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy.CanExecute(expression).Returns(true);
			findStrategy.Execute().Returns(new[] { new VaultServices.Entities.File.File { Id = 31 } });

			var findStrategy2 = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy2.CanExecute(expression).Returns(true);
			findStrategy2.Execute().Returns(new[] { new VaultServices.Entities.File.File { Id = 69 } });

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] { findStrategy, findStrategy2 });
			var findFiles = Container.Resolve<FindFiles>();

			Assert.IsTrue(findFiles.CanExecute(expression));
			Assert.AreEqual(2, findFiles.Execute().Count());
		}

		[Test]
		public void Execute_with_multiple_strategies_and_returning_the_same_files_returns_a_list_of_mergedfiles()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findStrategy = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy.CanExecute(expression).Returns(true);
			findStrategy.Execute().Returns(new[] { new VaultServices.Entities.File.File { Id = 31 } });

			var findStrategy2 = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			findStrategy2.CanExecute(expression).Returns(true);
			findStrategy2.Execute().Returns(new[] { new VaultServices.Entities.File.File { Id = 31 }, new VaultServices.Entities.File.File { Id = 66 } });

			Container.Provide<IEnumerable<IQueryOperation<VaultServices.Entities.File.File>>>(new[] { findStrategy, findStrategy2 });
			var findFiles = Container.Resolve<FindFiles>();

			Assert.IsTrue(findFiles.CanExecute(expression));
			CollectionAssert.AreEqual(findFiles.Execute().Select(f=>f.Id), new[] { 31 , 66 });
		}
	}
}