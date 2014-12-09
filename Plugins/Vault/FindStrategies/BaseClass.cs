using System.Collections.Generic;
using powerGateServer.SDK;

namespace UserServices.Vault.FindStrategies
{
	public abstract class FindVaultFiles
	{
		protected IExpression<Entities.File> Query;
		protected readonly IVaultConnection VaultConnection;

		protected FindVaultFiles(IVaultConnection vaultConnection)
		{
			VaultConnection = vaultConnection;
		}

		public virtual bool CanFind(IExpression<Entities.File> query)
		{
			Query = query;
			return true;
		}

		public abstract IEnumerable<Autodesk.Connectivity.WebServices.File> Find();
	}
}
