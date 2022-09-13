using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using powerGateServer.SDK;

namespace VaultServices.Entities.Base.FindStrategies
{
	public abstract class ExpressionParser<TFrom> : IExpressionParser<TFrom>
	{
		protected ParameterExpression CreateEntityParam<T>()
		{
			return Expression.Parameter(typeof(T), "e");
		}


		protected MethodCallExpression CreatWhere<T>(Expression lambda)
		{
			var queryable = Enumerable.Empty<T>().AsQueryable();
			var queryableExpression = Expression.Constant(queryable, queryable.GetType());
			var whereMethod = MethodOf(() => Queryable.Where(default(IQueryable<T>), default(Expression<Func<T, bool>>)));
			return Expression.Call(whereMethod, queryableExpression, lambda);
		}

		protected MethodInfo MethodOf<T>(Expression<Func<T>> method)
		{
			return ((MethodCallExpression)method.Body).Method;
		}

		public abstract IExpression<T> ParseFor<T>(IExpression<TFrom> propertyExpression) where T : IBaseObject;
	}
}