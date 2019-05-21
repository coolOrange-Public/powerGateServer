using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autodesk.Connectivity.WebServices;
using powerGateServer.Core.WcfFramework.Expressions.Where;
using powerGateServer.SDK;
using IVaultServices = coolOrange.VaultServices.Vault.IVaultServices;

namespace VaultServices.Entities.Base.FindStrategies
{
	public abstract class FindVaultEntityByProperties<T> : IQueryOperation<T> where T : IBaseObject
	{
		readonly PropertyNameSubstitution<T> _substitudePropertyName = new PropertyNameSubstitution<T>();
		readonly SearchConditionFactory _searchCondition = new SearchConditionFactory();
		readonly SearchSortingFactory _searchSorting = new SearchSortingFactory();

		readonly internal IVaultServices VaultServices;
		protected abstract string EntityType { get; }

		protected FindVaultEntityByProperties(IVaultServices vaultServices)
		{
			VaultServices = vaultServices;
		}

		HashSet<PropDef> _searchablePropertyDefinitions;
		internal HashSet<PropDef> SearchablePropertyDefinitions
		{
			get
			{
				return _searchablePropertyDefinitions ?? 
				       (_searchablePropertyDefinitions = new HashSet<PropDef>(VaultServices.GetAllPropertyDefinitions(EntityType)
					       .Where(p => p.IsBasicSrch)));
			}
		}
		internal IEnumerable<SrchCond> SearchConditions;
		internal IEnumerable<SrchSort> SearchSortings;

		public virtual bool CanExecute(IExpression<T> expression)
		{
			var propertiesFilter = GetPropertiesFilter(expression.Where);
			if (propertiesFilter != null)
			{
				SearchConditions = ToSearchConditions(propertiesFilter).ToList();
				SearchSortings = ToSearchSort(expression.OrderBy, true).ToList();
			}
			else
			{
				SearchConditions = ToSearchConditions(expression.Where).ToList();
				SearchSortings = ToSearchSort(expression.OrderBy, false).ToList();
			}
			return SearchConditions.Any();
		}

		public abstract IEnumerable<T> Execute();

		IEnumerable<SrchCond> ToSearchConditions(IEnumerable<IWhereToken<T>> whereExpression)
		{
			var whereTokens = whereExpression.ToList();
			if (!whereTokens.Any())
				yield return _searchCondition.CreateForAllEntities();
			foreach (var tokenExpression in whereTokens)
			{
				var whereProperty = GetPropertyDefinition(tokenExpression.PropertyName, false);
				if (whereProperty != null)
					yield return _searchCondition.Create(whereProperty, tokenExpression);
			}
		}
		IEnumerable<SrchCond> ToSearchConditions(IEnumerable<IWhereToken<Property.Property>> whereExpression)
		{
			var whereTokens = whereExpression.ToList();
			if (!whereTokens.Any())
				yield return _searchCondition.CreateForAllEntities();
			for (var i = 0; i < whereTokens.Count; i++)
			{
				var wherePropNameExpression = whereTokens.ElementAt(i);
				if (wherePropNameExpression.PropertyName == "Name")
				{
					if(wherePropNameExpression.Operator != OperatorType.Equals)
						throw new NotImplementedException("Currently it is possible to search for Property Names with equals (e.g. $filter=Name eq 'Author')");
					var whereProperty = GetPropertyDefinition((string)wherePropNameExpression.Value, true);
					if (whereProperty != null)
					{
						i++;
						var wherePropValueExpression = whereTokens.ElementAt(i);
						yield return _searchCondition.Create(whereProperty, wherePropValueExpression);
					}
				}
			}
		}

		IEnumerable<SrchSort> ToSearchSort(IOrderByClause<T> orderBy, bool sortByNavigationProperties)
		{
			foreach (var orderbyToken in orderBy.ToList())
			{
				var orderByProperty = GetPropertyDefinition(orderbyToken.PropertyName, sortByNavigationProperties);
				if (orderByProperty != null)
					yield return _searchSorting.Create(orderByProperty, orderbyToken);
			}
		}


		PropDef GetPropertyDefinition(string propertyName, bool searchByVaultPropertyName)
		{
			if (searchByVaultPropertyName)
				return SearchablePropertyDefinitions.FirstOrDefault(p => p.DispName == propertyName);
			try
			{
				var propertySystemName = _substitudePropertyName.ToVault(propertyName);
				return SearchablePropertyDefinitions.FirstOrDefault(p => p.SysName == propertySystemName);
			}
			catch (Exception)
			{
				return null;
			}
		}

		IEnumerable<IWhereToken<Property.Property>> GetPropertiesFilter(IWhereClause<T> where )
		{
			if(where.IsSet())
				try
				{
					dynamic whereExpression = where.Base;
					if (whereExpression.Body.Arguments[0].Member.Name == "Properties")
					{
						if (whereExpression.Body.Arguments.Count == 1)
							return Enumerable.Empty<IWhereToken<Property.Property>>();
						var unary = Expression.MakeUnary(ExpressionType.Quote, whereExpression.Body.Arguments[1], typeof(Property.Property));
						return new WhereClause<Property.Property>(unary);
					}
				}
				catch
				{
					// ignored
				}
			return null;
		}
	}
}