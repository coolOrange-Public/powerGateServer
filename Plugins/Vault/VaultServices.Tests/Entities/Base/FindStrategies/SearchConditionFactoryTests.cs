using Autodesk.Connectivity.WebServices;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base.FindStrategies;

namespace VaultServices.Tests.Entities.Base.FindStrategies
{
	[TestFixture]
	public class SearchConditionFactoryTests : ContainerBasedTests
	{
		[Test]
		public void Create_sets_PropDefId_of_injected_propertyDefinition()
		{
			var searchCondition = Container.Resolve<SearchConditionFactory>();

			var condition = searchCondition.Create(new PropDef { Id = 666 }, Substitute.For<IWhereToken<string>>());

			Assert.AreEqual(666,condition.PropDefId);
		}

		[Test]
		[TestCase(OperatorType.Contains, Result = 1)]
		[TestCase(OperatorType.DoesNotContain, Result = 2)]
		
		[TestCase(OperatorType.Equals, Result = 3)]
		[TestCase(OperatorType.NotEquals, Result = 10)]

		[TestCase(OperatorType.Empty, Result = 4)]
		[TestCase(OperatorType.NotEmpty, Result = 5)]

		[TestCase(OperatorType.GreatherThan, Result = 6)]
		[TestCase(OperatorType.GreatherThanOrEquals, Result = 7)]

		[TestCase(OperatorType.LessThan, Result = 8)]
		[TestCase(OperatorType.LessThanOrEquals, Result = 9)]

		public long Create_condition_with_correct_simple_vault_operatorType(OperatorType operatorType)
		{
			var whereToken = Substitute.For<IWhereToken<string>>();
			whereToken.Operator.Returns(operatorType);
			whereToken.Value.Returns("es (3)reicht");
			var searchCondition = Container.Resolve<SearchConditionFactory>();

			var condition = searchCondition.Create(new PropDef(), whereToken);

			Assert.AreEqual("es (3)reicht", condition.SrchTxt);
			Assert.AreEqual(PropertySearchType.SingleProperty, condition.PropTyp);
			return condition.SrchOper;
		}

		[Test]
		[TestCase(OperatorType.EndsWith, "*es (3)reicht", Result = 3)]
		[TestCase(OperatorType.DoesNotEndsWith, "*es (3)reicht", Result = 10)]

		[TestCase(OperatorType.StartsWith, "es (3)reicht*", Result = 3)]
		[TestCase(OperatorType.DoesNotStartWith,"es (3)reicht*", Result = 10)]
		public long Create_condition_with_complex_vault_operatorType_and_correct_search_value(OperatorType operatorType, string expextedSearchValue)
		{
			var whereToken = Substitute.For<IWhereToken<string>>();
			whereToken.Operator.Returns(operatorType);
			whereToken.Value.Returns("es (3)reicht");
			var searchCondition = Container.Resolve<SearchConditionFactory>();

			var condition = searchCondition.Create(new PropDef(), whereToken);

			Assert.AreEqual(expextedSearchValue, condition.SrchTxt);
			Assert.AreEqual(PropertySearchType.SingleProperty, condition.PropTyp);
			return condition.SrchOper;
		}
	}
}