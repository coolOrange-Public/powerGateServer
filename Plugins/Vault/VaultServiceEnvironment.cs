using Autofac;
using powerVault.Services;
using VaultServices.Environment;

namespace VaultServices
{
	public class VaultServiceEnvironment
	{
		public static IVaultEnvironment Load()
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterType<SystemImpl>().As<ISystem>();
			containerBuilder.RegisterType<VaultAssemblyLoader>().As<IAssemblyLoader>();
			containerBuilder.RegisterType<PowerVaultEnvironment>().As<IVaultEnvironment>();
			var container = containerBuilder.Build();
			var vaultEnvironment = container.Resolve<IVaultEnvironment>();
			return vaultEnvironment;
		}
	}
}