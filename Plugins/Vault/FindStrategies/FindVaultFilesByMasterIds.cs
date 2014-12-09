using System;
using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;

namespace UserServices.Vault.FindStrategies
{
	public class FindVaultFilesByMasterIds : FindVaultFiles
	{
		private IEnumerable<IWhereToken> _whereTokens;

		public FindVaultFilesByMasterIds(IVaultConnection connection)
			: base(connection)
		{
		}

		public override bool CanFind(IExpression<Entities.File> query)
		{
			base.CanFind(query);
			_whereTokens = Query.Where.Where(w => w.Property == "MasterId" && w.Operator == OperatorType.Equals);
			return _whereTokens.Any();
		}

		public override IEnumerable<Autodesk.Connectivity.WebServices.File> Find()
		{
			var vaultFiles = new List<Autodesk.Connectivity.WebServices.File>();
			var masterIds = _whereTokens.Select(w => (long)(int)w.Value);
			foreach (var masterId in masterIds)
				try
				{
					vaultFiles.Add(VaultConnection.WebServiceManager.DocumentService.GetLatestFileByMasterId(masterId));
				}
				catch (Exception)
				{
				}
			return vaultFiles;
		}
	}
}