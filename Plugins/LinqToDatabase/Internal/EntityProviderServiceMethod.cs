using System.Collections.Generic;
using IQToolkit;
using IQToolkit.Data;
using powerGateServer.SDK;

namespace LinqToDb.Internal
{
	public abstract class EntityProviderServiceMethod<T> : ServiceMethod<T>
	{
		protected DbEntityProvider EntityProvider;

		protected EntityProviderServiceMethod(DbEntityProvider entityProvider)
		{
			EntityProvider = entityProvider;
		}

		public override void Create(T entity)
		{
			Entities.Insert(entity);
		}

		public override void Delete(T entity)
		{
			Entities.Delete(entity);
		}

		public override IEnumerable<T> Query(IExpression<T> query)
		{
			var expressionVisitor = new powerGateServer.Core.WcfFramework.Expressions.ExpressionVisitor(query.Base);
			var getEntities = expressionVisitor.ReplaceConstant(e => e.Type == typeof(powerGateServer.Core.WcfFramework.Internal.Query<T>), Entities);
			System.Linq.Expressions.Expression<System.Func<IEnumerable<T>>> linqToDbExpression = System.Linq.Expressions.Expression.Lambda<System.Func<IEnumerable<T>>>(
				EntityProvider.GetExecutionPlan(getEntities), 
				new System.Linq.Expressions.ParameterExpression[0]);
			return linqToDbExpression.Compile()();
		}

		public override void Update(T entity)
		{
			Entities.Update(entity);
		}

		private IEntityTable<T> Entities
		{
			get
			{
				return EntityProvider.GetTable<T>();
			}
		}
	}
}