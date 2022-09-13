using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using powerGateServer.SDK;

namespace VaultServices.Entities.Base.FindStrategies
{
	public abstract class QueryNavigationProperty<T> : IQueryOperation<T>
	{
		public IQueryOperation<File.File> QueryFiles { get; set; }
		public IQueryOperation<Folder.Folder> QueryFolders { get; set; }
		
		readonly IExpressionParser<T> _expressionParser;
		protected IExpression<T> Expression;

		protected int TopCount;
		protected int SkipCount;

		protected abstract Func<IBaseObject, IEnumerable<T>> GetNavigationProperties { get; }

		protected QueryNavigationProperty(IExpressionParser<T> expressionParser)
		{
			_expressionParser = expressionParser;
		}

		public bool CanExecute(IExpression<T> expression)
		{
			Expression = expression;
			return CheckQueryOperation(QueryFiles, expression) | CheckQueryOperation(QueryFolders, expression);
		}

		bool CheckQueryOperation<TEntity>(IQueryOperation<TEntity> queryOperation, IExpression<T> propertyExpression) where TEntity : IBaseObject
		{
			if (queryOperation != null && propertyExpression != null)
			{
				var entityExpression = _expressionParser.ParseFor<TEntity>(propertyExpression);
				if (queryOperation.CanExecute(entityExpression))
					return true;
			}
			return false;
		}

		public IEnumerable<T> Execute()
		{
			SetCountVariables();
			var properties = new List<T>();
			if (ExecutionAllowed<File.File>())
				properties.AddRange(GetFilteredPropertiesByQueryOperation(QueryFiles));
			if (ExecutionAllowed<Folder.Folder>())
				properties.AddRange(GetFilteredPropertiesByQueryOperation(QueryFolders));
			return properties;
		}

		bool ExecutionAllowed<TEntity>() where TEntity : BaseObject
		{
			var result = GetAllOperators(typeof(TEntity).Name).Aggregate(true, (current, expressionCheck) =>
				current & CheckOperatorValueByPredicate("Type", expressionCheck.Key, expressionCheck.Value));
			return result && (TopCount == -1 || TopCount > 0) && (SkipCount == -1 || SkipCount > 0);
		}

		private IEnumerable<T> GetFilteredPropertiesByQueryOperation<TEntity>(IQueryOperation<TEntity> queryOperation) where TEntity : IBaseObject
		{
			var entities = queryOperation.Execute();
			var properties = new List<T>();
			foreach (var entity in entities)
				properties.AddRange(GetNavigationProperties(entity));
			return FilterPropertiesByExpression(properties);
		}

		private IEnumerable<T> FilterPropertiesByExpression(IEnumerable<T> allProperties)
		{
			var properties = allProperties.Where(IsPropertyValid).ToList();

			if (SkipCount > 0)
			{
				var skipped = properties.Count;
				if (SkipCount > properties.Count)
					properties.Clear();
				else
					properties = properties.Skip(SkipCount).ToList();
				SkipCount -= skipped;

			}
			if (TopCount > 0)
			{
				properties = properties.GetRange(0, TopCount > properties.Count ? properties.Count : TopCount);
				TopCount -= properties.Count;
			}
			return properties;
		}

		void SetCountVariables()
		{
			TopCount = Expression.TopCount;
			if (TopCount == 0)
				TopCount = -1;
			SkipCount = Expression.SkipCount;
			if (SkipCount == 0)
				SkipCount = -1;
		}
		private bool IsPropertyValid(T entityProperty)
		{
			return typeof(T).GetProperties().All(propertyInfo => IsPropertyInfoValid(propertyInfo, entityProperty));
		}

		private bool IsPropertyInfoValid(PropertyInfo propertyInfo, T entityProperty)
		{
			var validationItem = string.Empty;
			if (propertyInfo.GetValue(entityProperty) != null)
				validationItem = propertyInfo.GetValue(entityProperty).ToString();
			return GetAllOperators(validationItem).All(expressionCheck =>
				CheckOperatorValueByPredicate(propertyInfo.Name, expressionCheck.Key, expressionCheck.Value));
		}

		private bool CheckOperatorValueByPredicate(string propertyName, OperatorType operatorType, Predicate<string> check)
		{
			var tokens = Expression.Where.Where(token => token.PropertyName == propertyName && token.Operator == operatorType);
			return tokens.Aggregate(true, (current, whereToken) => current & check.Invoke(whereToken.Value.ToString()));
		}

		private Dictionary<OperatorType, Predicate<string>> GetAllOperators(string validation)
		{
			return new Dictionary<OperatorType, Predicate<string>>
            {
                {OperatorType.Contains, validation.Contains},
                {OperatorType.DoesNotContain, userInput => !validation.Contains(userInput)},
                {OperatorType.Equals, userInput => string.Equals(userInput, validation, StringComparison.InvariantCultureIgnoreCase)},
                {OperatorType.NotEquals, userInput => !string.Equals(userInput, validation, StringComparison.InvariantCultureIgnoreCase)},
                {OperatorType.EndsWith, validation.EndsWith},
                {OperatorType.DoesNotEndsWith, userInput => !validation.EndsWith(userInput)},
                {OperatorType.StartsWith, validation.StartsWith},
                {OperatorType.DoesNotStartWith, userInput => !validation.StartsWith(userInput)}
            };
		}
	}
}