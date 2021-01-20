using coolOrange.VaultServices.Vault;

namespace VaultServices.Environment
{
	public interface IVaultEnvironment
	{
		IVaultConnection Login(VaultLoginCredentials loginCredentials);
	}
}
