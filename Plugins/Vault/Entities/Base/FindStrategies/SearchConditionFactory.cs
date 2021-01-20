using System;
using Autodesk.Connectivity.WebServices;
using powerGateServer.SDK;

namespace VaultServices.Entities.Base.FindStrategies
{
	public class SearchConditionFactory
	{
		public enum OperatorType
		{
			Contains = 1,
			DoesNotContain = 2,
			Equals = 3,
			Empty = 4,
			NotEmpty = 5,
			GreatherThan = 6,
			GreatherThanOrEquals = 7,
			LessThan = 8,
			LessThanOrEquals = 9,
			NotEquals = 10,
		}

		public SrchCond CreateForAllEntities()
		{
			return new SrchCond {
				PropDefId = 0,
				PropTyp = PropertySearchType.AllProperties,
				SrchOper = (int) OperatorType.Contains,
				SrchRule = SearchRuleType.Must,
				SrchTxt = ""
			};
		}


		public SrchCond Create(PropDef propertyDefinition, IWhereToken<Property.Property> whereProperty)
		{
			return Create(propertyDefinition, (object)whereProperty);
		}

		public SrchCond Create<T>(PropDef propertyDefinition, IWhereToken<T> whereEntity)
		{
			return Create(propertyDefinition, (object)whereEntity);
		}

		SrchCond Create(PropDef propertyDefinition, dynamic whereEntity)
		{
			return new SrchCond
			{
				PropDefId = propertyDefinition.Id,
				PropTyp = PropertySearchType.SingleProperty,
				SrchOper = (int)ToSearchOperator(whereEntity.Operator),
				SrchRule = ToSearchRule(whereEntity.Rule),
				SrchTxt = GetSearchValue(whereEntity)
			};
		}

		string GetSearchValue<T>(IWhereToken<T> tokenExpression)
		{
			if (tokenExpression.Operator == powerGateServer.SDK.OperatorType.EndsWith || tokenExpression.Operator == powerGateServer.SDK.OperatorType.DoesNotEndsWith)
				return "*" + tokenExpression.Value;
			if (tokenExpression.Operator == powerGateServer.SDK.OperatorType.StartsWith || tokenExpression.Operator == powerGateServer.SDK.OperatorType.DoesNotStartWith)
				return tokenExpression.Value + "*";
			return tokenExpression.Value == null ? "" : tokenExpression.Value.ToString();
		}

		OperatorType ToSearchOperator(powerGateServer.SDK.OperatorType? operatorType)
		{
			if (operatorType == null)
				return OperatorType.Contains;
			if (operatorType == powerGateServer.SDK.OperatorType.EndsWith || operatorType == powerGateServer.SDK.OperatorType.StartsWith)
				return OperatorType.Equals;
			if (operatorType == powerGateServer.SDK.OperatorType.DoesNotEndsWith || operatorType == powerGateServer.SDK.OperatorType.DoesNotStartWith)
				return OperatorType.NotEquals;
			return (OperatorType)Enum.Parse(typeof(OperatorType), operatorType.ToString());
		}

		SearchRuleType ToSearchRule(LogicalOperator? rule)
		{
			return rule == LogicalOperator.Or ? SearchRuleType.May : SearchRuleType.Must;
		}
	}
}