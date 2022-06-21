using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autodesk.Connectivity.WebServices;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using powerVault.Cmdlets.Cmdlets.Vault.Facade.Files;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.Core.Tests;
using powerGateServer.SDK;
using VaultServices.Entities.File.FindStrategies;
using Folder = Autodesk.Connectivity.WebServices.Folder;

namespace VaultServices.Tests.Entities.File.FindStrategies
{
	[TestFixture]
	public class FindVaultFilesByPropertiesTests : ContainerBasedTests
	{
		readonly ExpressionFactory _expressionFactory = new ExpressionFactory();

		readonly IEnumerable<PropDef> _propertyDefinitions = new[] {
					new PropDef{Id = 1, IsBasicSrch = true,  DispName = "File Name",    SysName = "Name"},
					new PropDef{Id = 2, IsBasicSrch = true,  DispName = "President",    SysName = "{A9725F38-3ADC-4B7B-A3A0-35872A516440}"},
					new PropDef{Id = 3, IsBasicSrch = false, DispName = "FileSize",     SysName = "File Size"},
					new PropDef{Id = 4, IsBasicSrch = true,  DispName = "Sonstiges",    SysName = "{asdfasdf-3ADC-asdf-asdf-asdfasdfasdf}"},
					new PropDef{Id = 5, IsBasicSrch = true,  DispName = "Create User",  SysName = "CreateUserName"}};
		[Test]
		public void CanExecute_returns_true_when_searching_for_direct_file_properties_that_are_searchable()
		{
			Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>().GetAllPropertyDefinitions("FILE")
				.Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var whereName = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			whereName.PropertyName.Returns("Name");
			var wherePresident = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			wherePresident.PropertyName.Returns("CreateUser");
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.File.File>> { whereName, wherePresident }.GetEnumerator());

			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			Assert.IsTrue(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void CanExecute_returns_true_when_searching_for_properties_that_are_searchable()
		{
			Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>().GetAllPropertyDefinitions("FILE")
				.Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			expression.Where.IsSet().Returns(true);
			expression.Where.Base.Returns(
				CreateCallWhere<VaultServices.Entities.File.File>(
					file =>
						file.Properties.Any(
							p => p.Name == "President" && p.Value != "hallo"
								&& p.Name == "File Name" && p.Value != "hallo")));

			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			Assert.IsTrue(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void CanExecute_returns_true_when_no_filter_is_set_since_this_means_searching_for_all_files()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.File.File>>().GetEnumerator());
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();
			Assert.IsTrue(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void CanExecute_returns_true_when_no_property_filter_is_set_since_this_means_searching_for_all_files()
		{
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			expression.Where.IsSet().Returns(true);
			expression.Where.Base.Returns(
				CreateCallWhere<VaultServices.Entities.File.File>(
					file => file.Properties.Any()));
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();
			Assert.IsTrue(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void CanExectute_returns_false_when_searching_for_not_searchable_direct_properties()
		{
			Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>().GetAllPropertyDefinitions("FILE")
			   .Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var whereId = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			whereId.PropertyName.Returns("Id");
			var whereFileSize = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			whereFileSize.PropertyName.Returns("FileSize");
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.File.File>> { whereId, whereFileSize }.GetEnumerator());
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();
			Assert.IsFalse(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void CanExectute_returns_false_when_searching_for_not_searchable_properties()
		{
			Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>().GetAllPropertyDefinitions("FILE")
			   .Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			expression.Where.IsSet().Returns(true);
			expression.Where.Base.Returns(
				CreateCallWhere<VaultServices.Entities.File.File>(
					file =>
						file.Properties.Any(
							p => p.Name == "FileSize" && p.Value == "1000"
								|| p.Name == "Id" && p.Value == "666")));
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();
			Assert.IsFalse(findFilesByProperties.CanExecute(expression));
		}

		[Test]
		public void Execute_performs_vault_search_on_all_folders_recursive()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Any<IEnumerable<SrchCond>>(), Arg.Any<IEnumerable<SrchSort>>(), latestOnly: Arg.Any<bool>(),
				folders: null, recursive: true);
		}

		[Test]
		public void Execute_performs_vault_search_on_only_the_latest_file_versions()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Any<IEnumerable<SrchCond>>(), Arg.Any<IEnumerable<SrchSort>>(), Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(),
				latestOnly: true);
		}

		[Test]
		public void Execute_performs_vault_search_by_converting_the_direct_properties_to_conditions()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var whereName = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			whereName.PropertyName.Returns("Name");
			whereName.Operator.Returns(OperatorType.Contains);
			whereName.Value.Returns("Hallo");
			whereName.Rule.Returns(LogicalOperator.Or);
			var whereCreateUser = Substitute.For<IWhereToken<VaultServices.Entities.File.File>>();
			whereCreateUser.PropertyName.Returns("CreateUser");
			whereCreateUser.Operator.Returns(OperatorType.EndsWith);
			whereCreateUser.Value.Returns("mir");

			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.File.File>> { whereName, whereCreateUser }.GetEnumerator());
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Is<IEnumerable<SrchCond>>(conds =>
					conds.Count() == 2 &&
					conds.First().PropDefId == 1 && conds.First().PropTyp == PropertySearchType.SingleProperty && conds.First().SrchTxt == "Hallo" && conds.First().SrchOper == 1 && conds.First().SrchRule == SearchRuleType.May &&
					conds.Last().PropDefId == 5 && conds.Last().PropTyp == PropertySearchType.SingleProperty && conds.Last().SrchTxt == "*mir" && conds.Last().SrchOper == 3 && conds.Last().SrchRule == SearchRuleType.Must),
				Arg.Any<IEnumerable<SrchSort>>(), Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(), latestOnly: Arg.Any<bool>());
		}

		[Test]
		public void Execute_performs_vault_search_by_converting_the_properties_to_conditions()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			expression.Where.IsSet().Returns(true);
			expression.Where.Base.Returns(
					CreateCallWhere<VaultServices.Entities.File.File>(
						file =>
							file.Properties.Any(
								p => p.Name == "President" && p.Value != "Barack"
									&& p.Name == "Sonstiges" && p.Value == "heute schneit es regen")));
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Is<IEnumerable<SrchCond>>(conds =>
					conds.Count() == 2 &&
					conds.First().PropDefId == 2 && conds.First().PropTyp == PropertySearchType.SingleProperty && conds.First().SrchTxt == "Barack" && conds.First().SrchOper == 10 && conds.First().SrchRule == SearchRuleType.Must &&
					conds.Last().PropDefId == 4 && conds.Last().PropTyp == PropertySearchType.SingleProperty && conds.Last().SrchTxt == "heute schneit es regen" && conds.Last().SrchOper == 3 && conds.Last().SrchRule == SearchRuleType.Must),
				Arg.Any<IEnumerable<SrchSort>>(), Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(), latestOnly: Arg.Any<bool>());
		}

		[Test]
		public void Execute_performs_vault_search_with_one_condition_of_type_allProperties()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Is<IEnumerable<SrchCond>>(conds =>
					conds.Count() == 1 &&
					conds.First().PropDefId == 0 && conds.First().PropTyp == PropertySearchType.AllProperties && conds.First().SrchTxt == "" && conds.First().SrchOper == 1 && conds.First().SrchRule == SearchRuleType.Must),
				Arg.Any<IEnumerable<SrchSort>>(), Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(), latestOnly: Arg.Any<bool>());
		}

		[Test]
		public void Execute_performs_vault_search_with_sorting_on_expected_properties()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();

			var oderByName = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			oderByName.PropertyName.Returns("Name");
			var oderByCreateUser = Substitute.For<IOrderByToken<VaultServices.Entities.File.File>>();
			oderByCreateUser.PropertyName.Returns("CreateUser");
			oderByCreateUser.Method.Returns(OrderingMethod.ThenByDescending);
			expression.OrderBy.GetEnumerator().Returns(new List<IOrderByToken<VaultServices.Entities.File.File>> { oderByName, oderByCreateUser }.GetEnumerator());
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);
			findFilesByProperties.Execute();

			vault.Received()
				.FindFilesBySearchConditions(Arg.Any<IEnumerable<SrchCond>>(),
				Arg.Is<IEnumerable<SrchSort>>(sorts =>
					sorts.Count() == 2 &&
					sorts.First().PropDefId == 1 && sorts.First().SortAsc &&
					sorts.Last().PropDefId == 5 && !sorts.Last().SortAsc),
				Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(), latestOnly: Arg.Any<bool>());
		}

		[Test]
		public void Execute_returns_the_entityFiles_foreach_vaultFile_found_by_the_search()
		{
			var vault = Container.SubstituteFor<coolOrange.VaultServices.Vault.IVaultServices>();
			vault.GetAllPropertyDefinitions("FILE").Returns(_propertyDefinitions);

			var vaultFiles = new[]
			{
				new Autodesk.Connectivity.WebServices.File{Name = "Max.ipt"},
				new Autodesk.Connectivity.WebServices.File{Name = "Vladimir.ipt"},
				new Autodesk.Connectivity.WebServices.File{Name = "Barack.ipt"}
			};
			var entityFactory = Substitute.For<IVaultEntityFactory>();
			Container.Provide(new FileConversionContext { EntityFactory = entityFactory });

			foreach (var t in vaultFiles)
			{
				var entityFile = Substitute.For<VaultFile>();
				entityFactory.Create(t).Returns(entityFile);
				entityFile.Base.Returns(new FileIteration(null, t));
			}
			vault.FindFilesBySearchConditions(Arg.Any<IEnumerable<SrchCond>>(), Arg.Any<IEnumerable<SrchSort>>(), Arg.Any<IEnumerable<Folder>>(), recursive: Arg.Any<bool>(), latestOnly: Arg.Any<bool>())
				.Returns(vaultFiles);
			var expression = Substitute.For<IExpression<VaultServices.Entities.File.File>>();
			var findFilesByProperties = Container.Resolve<FindVaultFilesByProperties>();

			findFilesByProperties.CanExecute(expression);

			CollectionAssert.AreEquivalent(new[] { "Max.ipt", "Vladimir.ipt", "Barack.ipt" }, findFilesByProperties.Execute().Select(f => f.Name));
		}

		Expression<Func<VaultServices.Entities.File.File, bool>> CreateCallWhere<T>(Expression<Func<T, bool>> predicate)
		{
			var whereCall = _expressionFactory.CreateCallWhere(predicate);
			return (whereCall.Arguments[1] as UnaryExpression).Operand as Expression<Func<VaultServices.Entities.File.File, bool>>;
		}
	}
}
