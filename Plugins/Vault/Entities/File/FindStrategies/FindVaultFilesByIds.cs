using System.Collections.Generic;
using System.Linq;
using powerVault.Cmdlets.Cmdlets.Vault.Facade.Files;
using powerGateServer.SDK;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Entities.File.FindStrategies
{
	public class FindVaultFilesByIds : FindFilesBase
	{
		private readonly IVaultFileSearcher _vaultFileSearcher;
		private IEnumerable<IWhereToken<File>> _whereTokens;

		public FindVaultFilesByIds(IVaultFileSearcher vaultFileSearcher, FileConversionContext fileConversionContext)
			: base(fileConversionContext)
		{
			_vaultFileSearcher = vaultFileSearcher;
			_whereTokens = Enumerable.Empty<IWhereToken<File>>();
		}

		protected override bool CanExecute(IExpression<File> expression)
		{
			_whereTokens = expression.Where.Where(w => w.PropertyName == "Id" && w.Operator == OperatorType.Equals);
			return _whereTokens.Any();
		}

		protected override IEnumerable<Vault.File> Execute()
		{
			var ids = GetIds().ToList();
			var files = new List<Vault.File>();
			if (ids.Any())
				files.AddRange(ids.Select(id => _vaultFileSearcher.SearchById(id)));
			return files.Where(file => file != default(Vault.File));
		}

		private IEnumerable<long> GetIds()
		{
			return _whereTokens.Select(expression => long.Parse(expression.Value.ToString()))
				.GroupBy(id => id).Select(groups => groups.Key);
		}
	}
}