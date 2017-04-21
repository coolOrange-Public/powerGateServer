using Autofac;
using powerVault.Services;
using powerVault.Services.Vault;
using VaultServices.Environment;

namespace VaultServices
{
	public class VaultServiceEnvironment
	{
		public static IVaultEnvironment Load()
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterType<SystemImpl>().As<ISystem>();
			containerBuilder.RegisterType<AssemblyResolver>().As<IAssemblyResolver>();
			containerBuilder.RegisterType<PowerVaultAssemblyLoader>().As<IAssemblyLoader>();
			containerBuilder.RegisterType<VaultAssemblyLoader>().As<IAssemblyLoader>();
			containerBuilder.RegisterType<PowerVaultEnvironment>().As<IVaultEnvironment>();
			containerBuilder.RegisterType<PowerVaultEnvironment>().As<IVaultEnvironment>();
			var container = containerBuilder.Build();
			var vaultEnvironment = container.Resolve<IVaultEnvironment>();
			return vaultEnvironment;
		}
	}
}