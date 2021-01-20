using coolOrange.VaultServices.Vault;

namespace VaultServices.Environment
{
	public interface IAssemblyLoader
	{
		void AddResolver();
	}

	public class VaultAssemblyLoader : IAssemblyLoader
	{
		public void AddResolver()
		{
			VaultEnvironment.Instance.Initialize();
		}
	}
}
