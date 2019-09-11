using System.Collections.Generic;
using System.Linq;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using coolOrange.VaultServices.Vault;
using IVaultConnection = coolOrange.VaultServices.Vault.IVaultConnection;

namespace VaultServices.Environment
{
	public class PowerVaultEnvironment : IVaultEnvironment
	{
		internal readonly VaultEnvironment Environment;

		public PowerVaultEnvironment(IEnumerable<IAssemblyLoader> assemblyLoaders)
		{
			assemblyLoaders.ToList().ForEach(loader => loader.AddResolver());
			Environment = new VaultEnvironment();
		}

		public IVaultConnection Login(VaultLoginCredentials loginCredentials)
		{
			Autodesk.DataManagement.Client.Framework.Library.Initialize(false);
			return Environment.Login(loginCredentials);
		}

		internal class VaultEnvironment : IVaultEnvironment
		{
			powerVault.Cmdlets.Cmdlets.Vault.Facade.IVaultConnection _vault;
			internal powerVault.Cmdlets.Cmdlets.Vault.Facade.IVaultConnection Vault
			{
				get { return _vault ?? (_vault = new VaultPowerShellProxy()); }
				set { _vault = value; }
			}

			public IVaultConnection Login(VaultLoginCredentials loginCredentials)
			{
				Vault.Login(loginCredentials.User, loginCredentials.Password, loginCredentials.Server, loginCredentials.Vault);
				return new ExistingVaultConnection(Vault.Connection);
			}
		}
	}
}