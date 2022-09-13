using System;
using System.CodeDom;
using System.Linq;
using System.Linq.Expressions;
using powerGateServer.Core.WcfFramework.Expressions;
using powerGateServer.SDK;
using VaultServices.Entities.Base.FindStrategies;
using ExpressionVisitor = System.Linq.Expressions.ExpressionVisitor;

namespace VaultServices.Entities.Link.FindStrategies
{
	public class LinkExpressionParser : ExpressionParser<Link>
	{
		public override IExpression<T> ParseFor<T>(IExpression<Link> propertyExpression)
		{
			var linkExpressionModifier = new LinkExpressionModifier<T>(this);
			var entityExpression = linkExpressionModifier.Modify(propertyExpression.Base);
			return new RequestExpression<T>(entityExpression);
		}

		class LinkExpressionModifier<T> : ExpressionVisitor
		{
			readonly LinkExpressionParser _expressionParser;
			readonly ParameterExpression _entityParam;

			public LinkExpressionModifier(LinkExpressionParser expressionParser)
			{
				_expressionParser = expressionParser;
				_entityParam = expressionParser.CreateEntityParam<T>();
			}

			public Expression Modify(Expression expression)
			{
				return Visit(expression);
			}

			protected override Expression VisitMember(MemberExpression node)
			{
				if (node.Member.Name == "ParentId" || node.Member.Name == "ChildId")
					return Expression.Property(_entityParam, "Id");
				if (node.Member.Name == "ParentType" || node.Member.Name == "ChildType")
					return Expression.Property(_entityParam, "Type");
				return base.VisitMember(node);
			}

			protected override Expression VisitParameter(ParameterExpression node)
			{
				return _entityParam;
			}

			protected override Expression VisitLambda<T1>(Expression<T1> node)
			{
				if (node.Parameters[0].Type != typeof (T))
				{
					var type = typeof(Func<,>).MakeGenericType(typeof(T), node.ReturnType);
					var test = Expression.Lambda(type, node.Body, _entityParam);
					return Visit(test);
				}
				//if (node.ReturnType == typeof(bool))
				//	return base.VisitLambda(Expression.Lambda<Func<T, bool>>(node.Body, _entityParam));
				//if (node.ReturnType == typeof(long))
				//	return base.VisitLambda(Expression.Lambda<Func<T, long>>(node.Body, _entityParam));
				
				return base.VisitLambda(node);
			}

			protected override Expression VisitConstant(ConstantExpression node)
			{
				if (node.Value is IQueryable<Link>)
				{
					var queryable = Enumerable.Empty<T>().AsQueryable();
					return Expression.Constant(queryable, queryable.GetType());
				}
				return base.VisitConstant(node);
			}

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.Name == "Where")
				{
					var entity = Visit(node.Arguments[0]);
					var lambda = Visit(node.Arguments[1]);
					var whereMethod = _expressionParser.MethodOf(() => Queryable.Where(default(IQueryable<T>), default(Expression<Func<T, bool>>)));
					node = Expression.Call(whereMethod, entity, lambda);
				}
				if (node.Method.Name == "OrderBy")
				{
					var entity = Visit(node.Arguments[0]);
					var lambda = Visit(node.Arguments[1]);
					var whereMethod = _expressionParser.MethodOf(() => Queryable.OrderBy(default(IQueryable<T>), default(Expression<Func<T, long>>)));
					node = Expression.Call(whereMethod, entity, lambda);
				}
				if (node.Method.Name == "ThenBy")
				{
					var entity = Visit(node.Arguments[0]);
					var lambda = Visit(node.Arguments[1]);
					var whereMethod = _expressionParser.MethodOf(() => Queryable.ThenBy(default(IOrderedQueryable<T>), default(Expression<Func<T, long>>)));
					node = Expression.Call(whereMethod, entity, lambda);
				}
				return base.VisitMethodCall(node);
			}

			protected override Expression VisitUnary(UnaryExpression node)
			{
				if (node.Type == typeof (Expression<Func<Link, bool>>))
					node = Expression.MakeUnary(node.NodeType, node.Operand, typeof(Expression<Func<T, bool>>));
				return base.VisitUnary(node);
			}
		}
	}
}