using System.Collections.Generic;
using System.Linq;
using Autodesk.Connectivity.WebServices;
using powerGateServer.SDK;

namespace UserServices.Vault.FindStrategies
{
	public class FindVaultFilesByProperties : FindVaultFiles
	{
		private readonly IEntityConverter _entityConverter;

		public FindVaultFilesByProperties(IVaultConnection connection, IEntityConverter converter)
			: base(connection)
		{
			_entityConverter = converter;
		}


		public override bool CanFind(IExpression<Entities.File> query)
		{
			if (   query.Where.Any() 
			       && query.Where.All(w => new[] { "MasterId", "Id" }.Contains(w.PropertyName) 
			                               && w.Operator == OperatorType.Equals
			                               && new LogicalOperator?[] { LogicalOperator.And, null }.Contains(w.Rule)))
				return false;
			return base.CanFind(query); ;
		}

		public override IEnumerable<File> Find()
		{
			var srcCnds = _entityConverter.ToSearchConditions(Query.Where);

			if (srcCnds.Count() != Query.Where.Count())
				srcCnds = Enumerable.Empty<SrchCond>();

			var srcSort = _entityConverter.ToSearchSort(Query.OrderBy);
			var files = VaultConnection.SearchFiles(srcCnds, srcSort, Query.TopCount);
			return files;
		}
	}
}