using System;
using System.Linq;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using powerVault.Cmdlets.Cmdlets.Vault.Facade.Files;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using powerGateServer.Core.Tests;
using powerGateServer.Core.WcfFramework.Expressions;
using VaultServices.Entities.Base;
using VaultServices.Entities.File.FindStrategies;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Tests.Entities.File.FindStrategies
{
	[TestFixture]
	public class FindVaultFilesByIdsTests : ContainerBasedTests
	{
		readonly ExpressionFactory _expressionFactory = new ExpressionFactory();
		[SetUp]
		public void SetUp()
		{
			var factory = Substitute.For<IVaultEntityFactory>();
			factory.Create(Arg.Any<Vault.File>()).Returns(arg =>
			{
				var vaultFile = Substitute.For<VaultFile>();
				vaultFile.Base
					.Returns(new FileIteration(null, new Vault.File
					{
						Id = arg.Arg<Vault.File>().Id,
						Name = "Chuck Norris",
						CreateUserName = "Chuck Norris",
						CreateDate = DateTime.MinValue
					}));
				return vaultFile;
			});
			Container.Provide(new FileConversionContext { EntityFactory = factory });
		}

		[Test]
		public void Test_CanExecute_ExpressionEqualsId_ReturnsTrue()
		{
			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id == 31L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsTrue(findVaultFilesByIds.CanExecute(requestExpression));
		}

		[Test]
		public void Test_CanExecute_ExpressionNotAboutId_ReturnsFalse()
		{
			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Name == "MuhDuKuh");
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsFalse(findVaultFilesByIds.CanExecute(requestExpression));
		}

		[Test]
		public void Test_CanExecute_ExpressionNotEqualsId_ReturnsFalse()
		{
			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id != 31L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsFalse(findVaultFilesByIds.CanExecute(requestExpression));
			CollectionAssert.IsEmpty(findVaultFilesByIds.Execute());
		}

		[Test]
		public void Test_CanExecute_ExpressionEqualsIdNotAndTrue_ReturnsFalse()
		{
			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id != 31L && file.Id < 31L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsFalse(findVaultFilesByIds.CanExecute(requestExpression));
		}

		[Test]
		public void Test_Execute_CallingCanExecuteBeforeTrue_ReturnsOneFile()
		{
			var vaultFileSearcher = Container.SubstituteFor<IVaultFileSearcher>();
			vaultFileSearcher.SearchById(Arg.Any<long>()).Returns(arg => new Vault.File { Id = arg.Arg<long>() });

			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id == 31L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsTrue(findVaultFilesByIds.CanExecute(requestExpression));
			Assert.AreEqual(31L, findVaultFilesByIds.Execute().First().Id);
		}

		[Test]
		public void Test_Execute_ExpressionWithIdEqualsTwiceWithSameId_ReturnsOneFileAndApiGetsCalledJustOnce()
		{
			var vaultFileSearcher = Container.SubstituteFor<IVaultFileSearcher>();
			vaultFileSearcher.SearchById(Arg.Any<long>()).Returns(arg => new Vault.File { Id = arg.Arg<long>() });

			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id == 31L && file.Id == 31L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsTrue(findVaultFilesByIds.CanExecute(requestExpression));

			var files = findVaultFilesByIds.Execute().ToList();
			Assert.AreEqual(1, files.Count());
			Assert.AreEqual(31, files.First().Id);
			vaultFileSearcher.Received(1).SearchById(Arg.Any<long>());
		}

		[Test]
		public void Test_Execute_ExpressionWithThreeDifferentIdEqualsAndOneIsNotValid_ReturnsTwoFiles()
		{
			var vaultFileSearcher = Container.SubstituteFor<IVaultFileSearcher>();
			vaultFileSearcher.SearchById(Arg.Any<long>()).Returns(
				delegate (CallInfo arg)
				{
					if (arg.Arg<long>() == 3L)
						return default(Vault.File);
					return new Vault.File { Id = arg.Arg<long>() };
				});

			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id == 1L || file.Id == 2L || file.Id == 3L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsTrue(findVaultFilesByIds.CanExecute(requestExpression));

			var files = findVaultFilesByIds.Execute().ToList();
			Assert.AreEqual(2, files.Count());
			CollectionAssert.Contains(files.Select(file => file.Id), 1);
			CollectionAssert.Contains(files.Select(file => file.Id), 2);
			vaultFileSearcher.Received(3).SearchById(Arg.Any<long>());
		}

		[Test]
		public void Test_Execute_ExpressionWithWrongIdEquals_ReturnsEmptyList()
		{
			var vaultFileSearcher = Container.SubstituteFor<IVaultFileSearcher>();
			vaultFileSearcher.SearchById(Arg.Any<long>()).Returns(default(Vault.File));

			var expression = _expressionFactory.CreateCallWhere<VaultServices.Entities.File.File>(file => file.Id == 69L);
			var requestExpression = new RequestExpression<VaultServices.Entities.File.File>(expression);

			IQueryOperation<VaultServices.Entities.File.File> findVaultFilesByIds = Container.Resolve<FindVaultFilesByIds>();
			Assert.IsTrue(findVaultFilesByIds.CanExecute(requestExpression));

			CollectionAssert.IsEmpty(findVaultFilesByIds.Execute());
			vaultFileSearcher.Received(1).SearchById(Arg.Any<long>());
		}
	}
}