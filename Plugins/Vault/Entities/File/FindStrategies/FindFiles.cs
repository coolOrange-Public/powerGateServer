using System.Collections.Generic;
using System.Linq;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;
using VaultServices.Entities.Base;

namespace VaultServices.Entities.File.FindStrategies
{
	public class FindFiles : IQueryOperation<File>
	{
		private readonly IEnumerable<IQueryOperation<File>> _strategies;
		private IEnumerable<IQueryOperation<File>> _validStrategies;

		public FindFiles(IEnumerable<IQueryOperation<File>> strategies)
		{
			_strategies = strategies;
		}

		public bool CanExecute(IExpression<File> expression)
		{
			_validStrategies = _strategies.Where(s => s.CanExecute(expression)).ToList();
			return _validStrategies.Any();
		}

		public IEnumerable<File> Execute()
		{
			return _validStrategies.Any()
				? _validStrategies.SelectMany(o => o.Execute()).DistinctBy(file => file.Id).ToList()
				: Enumerable.Empty<File>();
		}
	}
}