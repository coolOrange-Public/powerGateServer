using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using powerGateServer.Core.WcfFramework.Expressions;
using powerGateServer.SDK;
using VaultServices.Entities.Base.FindStrategies;

namespace VaultServices.Entities.Property.FindStrategies
{
	public class PropertyExpressionParser : ExpressionParser<Property>
	{
		public override IExpression<T> ParseFor<T>(IExpression<Property> propertyExpression)
		{
			var anyCall = CreatePropertyAny<T>(propertyExpression);
			var entityExpression = CreatWhere<T>(anyCall);
            entityExpression = ExtractTopCountToTake<T>(propertyExpression, entityExpression);
            return new RequestExpression<T>(entityExpression);
        }

        private Expression<Func<T, bool>> CreatePropertyAny<T>(IExpression<Property> propertyExpression)
        {
	        var entityParam = CreateEntityParam<T>();
			var propertiesExpression = Expression.Property(entityParam, "Properties");
            var callAnyMethod = CreateAnyMethod<T>(propertyExpression, propertiesExpression);
			return Expression.Lambda<Func<T, bool>>(callAnyMethod, entityParam);
        }
			
        private MethodCallExpression CreateAnyMethod<T>(IExpression<Property> propertyExpression, MemberExpression propertiesExpression)
			{
            if (propertyExpression.Where.IsSet())
                return Expression.Call(MethodOf(() => Enumerable.Any<Property>(default(IEnumerable<Property>), default(Func<Property, bool>))),
                    propertiesExpression, propertyExpression.Where.Base);
            return Expression.Call(MethodOf(() => Enumerable.Any<Property>(default(IEnumerable<Property>))), propertiesExpression);
			}

			
        private MethodCallExpression ExtractTopCountToTake<T>(IExpression<Property> propertyExpression, MethodCallExpression entityExpression)
        {
            if (propertyExpression.TopCount > 0)
                entityExpression = Expression.Call(MethodOf(() => Queryable.Take(default(IQueryable<T>), default(int))), entityExpression, Expression.Constant(propertyExpression.TopCount));
            return entityExpression;
		}
	}
}