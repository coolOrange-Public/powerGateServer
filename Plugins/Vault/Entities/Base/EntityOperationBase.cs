using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using powerGateServer.SDK;
using powerGateServer.Core.ReflectionFramework;
using VaultServices.Extensions;

namespace VaultServices.Entities.Base
{
	public abstract class EntityOperationBase<T> : ServiceMethod<T>, IOperationHandler<T>
	{
		protected string[] KeyNames = GetKeyNames().ToArray();

		public IQueryOperation<T> QueryOperations { get; set; }
		public ICreateOperation<T> CreateOperations { get; set; }
		public IDeleteOperation<T> DeleteOperations { get; set; }
		public IUpdateOperation<T> UpdateOperations { get; set; }

		public override IEnumerable<T> Query(IExpression<T> expression)
		{
			if (QueryOperations == null || !QueryOperations.CanExecute(expression))
				return Enumerable.Empty<T>();

			var entities = QueryOperations.Execute();
			return entities.GroupByMany(KeyNames)
			   .Select(grouping => grouping.First());
		}

		public override void Update(T entity)
		{
			throw new NotImplementedException();
		}

		public override void Create(T entity)
		{
			throw new NotImplementedException();
		}

		public override void Delete(T entity)
		{
			throw new NotImplementedException();
		}

		static IEnumerable<string> GetKeyNames()
		{
			var dataServiceKeysAttributes = typeof(T).GetAttribute<DataServiceKeyAttribute>();
			return dataServiceKeysAttributes.KeyNames.Select(typeof(T).GetProperty)
				.Select(e => e.Name);
		}
	}
}