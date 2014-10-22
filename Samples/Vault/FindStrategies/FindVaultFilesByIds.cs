using System;
using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;

namespace UserServices.Vault.FindStrategies
{
	public class FindVaultFilesByIds : FindVaultFiles
	{
		private IEnumerable<IWhereToken> _whereTokens;

		public FindVaultFilesByIds(IVaultConnection connection)
			: base(connection)
		{
		}

		public override bool CanFind(IExpression<Entities.File> query)
		{
			base.CanFind(query);
			_whereTokens = Query.Where.Where(w => w.Property == "Id" && w.Operator == OperatorType.Equals);
			return _whereTokens.Any();
		}

		public override IEnumerable<Autodesk.Connectivity.WebServices.File> Find()
		{
			var vaultFiles = new List<Autodesk.Connectivity.WebServices.File>();
			var ids = _whereTokens.Select(w => (long)(int)w.Value);
			foreach (var id in ids)
				try
				{
					vaultFiles.Add(VaultConnection.WebServiceManager.DocumentService.GetFileById(id));
				}
				catch (Exception)
				{
				}
			return vaultFiles;
		}
	}
}