using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using powerVault.Cmdlets.Cmdlets.Vault.Facade.Files;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.File.FindStrategies;
using Entity = VaultServices.Entities;
using FindFilesBase = VaultServices.Entities.File.FindStrategies.FindFilesBase;

namespace VaultServices.Tests.Entities.File.FindStrategies
{
	[TestFixture]
	public class FindFilesBaseTests : ContainerBasedTests
	{
		[Test]
		public void Execute_calls_execute_from_derived_class_and_converts_vaultFiles_to_entityFiles()
		{
			var expression = Substitute.For<IExpression<Entity.File.File>>();
			var vaultFile = CreateVaultFile();
			var factory = Container.SubstituteFor<IVaultEntityFactory>();
			factory.Create(Arg.Any<Autodesk.Connectivity.WebServices.File>()).Returns(vaultFile);

			IQueryOperation<Entity.File.File> findFiles =
				new TestFindFilesBase(new FileConversionContext { EntityFactory = factory });
			findFiles.CanExecute(expression);

			var result = findFiles.Execute().First();
			Assert.AreEqual(666, result.Id);
			Assert.AreEqual("Spongebob.iam", result.Name);
			Assert.AreEqual(new DateTime(1999, 5, 1), result.CreateDate);
			Assert.AreEqual("Stephen Hillenburg", result.CreateUser);
			Assert.AreEqual("File", result.Type);
		}

		[Test]
		public void Execute_converts_the_properties_of_the_file_only_on_demand()
		{
			var onDemandExp = Substitute.For<ILazyLoad<Entity.File.File, IEnumerable<Entity.Property.Property>>>();
			var expression = Substitute.For<IExpression<Entity.File.File>>();
			expression.Expand.OnNavigationPropertyDemand(
					Arg.Any<Expression<Func<Entity.File.File, IEnumerable<Entity.Property.Property>>>>())
				.Returns(onDemandExp);

			var vaultFile = CreateVaultFile();
			var factory = Container.SubstituteFor<IVaultEntityFactory>();
			factory.Create(Arg.Any<Autodesk.Connectivity.WebServices.File>()).Returns(vaultFile);

			IQueryOperation<Entity.File.File> findFiles =
				new TestFindFilesBase(new FileConversionContext { EntityFactory = factory });
			findFiles.CanExecute(expression);
			findFiles.Execute();
			onDemandExp.Received().Call(Arg.Any<Func<Entity.File.File, IEnumerable<Entity.Property.Property>>>());
		}

		[Test]
		public void Execute_converts_vaultProperties_to_entityProperties_correctly()
		{
			var file = new Entity.File.File { Id = 666 };
			var vaultFile = CreateVaultFile();
			var expression = Substitute.For<IExpression<Entity.File.File>>();
			var lazyLoadExp = Substitute.For<ILazyLoad<Entity.File.File, IEnumerable<Entity.Property.Property>>>();
			expression.Expand.OnNavigationPropertyDemand(
					Arg.Any<Expression<Func<Entity.File.File, IEnumerable<Entity.Property.Property>>>>())
				.Returns(lazyLoadExp);
			lazyLoadExp.When(l => l.Call(Arg.Any<Func<Entity.File.File, IEnumerable<Entity.Property.Property>>>()))
				.Do(
					info =>
					{
						file.Properties = info.Arg<Func<Entity.File.File, IEnumerable<Entity.Property.Property>>>().Invoke(file);
					});
			var factory = Container.SubstituteFor<IVaultEntityFactory>();
			factory.Create(Arg.Any<Autodesk.Connectivity.WebServices.File>()).Returns(vaultFile);

			IQueryOperation<Entity.File.File> findFiles =
				new TestFindFilesBase(new FileConversionContext { EntityFactory = factory });
			findFiles.CanExecute(expression);
			findFiles.Execute();
			var prop = file.Properties.Last();
			Assert.AreEqual("Hobby", prop.Name);
			Assert.AreEqual(666, prop.ParentId);
			Assert.AreEqual("File", prop.ParentType);
			Assert.AreEqual("Jelly Fishing", prop.Value);
			Assert.AreEqual(5, file.Properties.Count());
		}

		[Test]
		public void Execute_gets_and_creates_children_onDemand()
		{
			var file = new Entity.File.File { Id = 666 };
			var vaultFile = CreateVaultFile();
			var expression = Substitute.For<IExpression<Entity.File.File>>();
			var lazyLoadExp = Substitute.For<ILazyLoad<Entity.File.File, IEnumerable<Entity.Link.Link>>>();
			expression.Expand.OnNavigationPropertyDemand(
					Arg.Is<Expression<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>>(
						expr => expr.ToString() == "file => file.Children"))
				.Returns(lazyLoadExp);
			lazyLoadExp.When(l => l.Call(Arg.Any<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>()))
				.Do(info => { file.Children = info.Arg<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>().Invoke(file); });
			var factory = Container.SubstituteFor<IVaultEntityFactory>();
			factory.Create(Arg.Any<Autodesk.Connectivity.WebServices.File>()).Returns(vaultFile);
			var assocService = Container.SubstituteFor<IAssociationService<Entity.File.File>>();
			assocService.GetChildren(file).Returns(new[] { new Entity.Link.Link { ParentId = 666 } });


			IQueryOperation<Entity.File.File> findFiles =
				new TestFindFilesBase(new FileConversionContext { EntityFactory = factory, AssociationService = assocService });
			findFiles.CanExecute(expression);
			findFiles.Execute();
			Assert.AreEqual(666, file.Children.First().ParentId);
		}

		[Test]
		public void Execute_gets_and_creates_parents_onDemand()
		{
			var file = new Entity.File.File { Id = 666 };
			var vaultFile = CreateVaultFile();
			var expression = Substitute.For<IExpression<Entity.File.File>>();
			var lazyLoadExp = Substitute.For<ILazyLoad<Entity.File.File, IEnumerable<Entity.Link.Link>>>();
			expression.Expand.OnNavigationPropertyDemand(
					Arg.Is<Expression<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>>(
						expr => expr.ToString() == "file => file.Parents"))
				.Returns(lazyLoadExp);
			lazyLoadExp.When(l => l.Call(Arg.Any<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>()))
				.Do(info => { file.Parents = info.Arg<Func<Entity.File.File, IEnumerable<Entity.Link.Link>>>().Invoke(file); });
			var factory = Container.SubstituteFor<IVaultEntityFactory>();
			factory.Create(Arg.Any<Autodesk.Connectivity.WebServices.File>()).Returns(vaultFile);
			var assocService = Container.SubstituteFor<IAssociationService<Entity.File.File>>();
			assocService.GetParents(file).Returns(new[] { new Entity.Link.Link { ParentId = 666 } });


			IQueryOperation<Entity.File.File> findFiles =
				new TestFindFilesBase(new FileConversionContext { EntityFactory = factory, AssociationService = assocService });
			findFiles.CanExecute(expression);
			findFiles.Execute();
			Assert.AreEqual(666, file.Parents.First().ParentId);
		}

		public class TestFindFilesBase : FindFilesBase
		{
			public TestFindFilesBase(FileConversionContext fileConversionContext)
				: base(fileConversionContext)
			{
			}

			protected override IEnumerable<Autodesk.Connectivity.WebServices.File> Execute()
			{
				return new[] { new Autodesk.Connectivity.WebServices.File() };
			}

			protected override bool CanExecute(IExpression<Entity.File.File> expression)
			{
				return true;
			}
		}

		VaultFile CreateVaultFile()
		{
			var props = new Properties
			{
				CreateProp("Name", "Spongebob.iam"),
				CreateProp("CreateUserName", "Stephen Hillenburg"),
				CreateProp("CreationDate", new DateTime(1999, 5, 1)),
				CreateProp("BestFriend", "Patrick Star"),
				CreateProp("Hobby", "Jelly Fishing")
			};

			var vaultFile = Substitute.For<VaultFile>();
			vaultFile.Identifiers.Id.Returns(666);
			vaultFile.Properties.Returns(props);

			vaultFile.Base.Returns(new FileIteration(null, new Autodesk.Connectivity.WebServices.File
			{
				Name = "Spongebob.iam",
				CreateDate = new DateTime(1999, 5, 1),
				CreateUserName = "Stephen Hillenburg",
				Id = 666
			})
			);
			return vaultFile;
		}

		IProperty CreateProp(string name, object value)
		{
			var prop = Substitute.For<IProperty>();
			prop.Definition.Returns(new PropertyDefinition(name) { DisplayName = name });
			prop.Value.Returns(value);
			prop.DisplayName.Returns(name);
			return prop;
		}
	}
}