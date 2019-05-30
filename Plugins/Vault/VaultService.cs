using System.Configuration;
using System.Reflection;
using Autofac;
using powerGateServer.SDK;
using VaultServices.Entities.File;
using VaultServices.Entities.Link;
using VaultServices.Environment;
using Module = Autofac.Module;
using Properties = VaultServices.Entities.Property.Properties;
using Property = VaultServices.Entities.Property.Property;
using VaultLoginCredentials = VaultServices.Environment.VaultLoginCredentials;

namespace VaultServices
{
	[WebServiceData("PGS", "Vault")]
	public class VaultService : ContainerBasedService
	{
		static readonly IVaultEnvironment Environment = VaultServiceEnvironment.Load();
		readonly Configuration _configuration = GetConfiguration();

		public VaultService()
		{
			var loginCredentials = new VaultLoginCredentials(_configuration.AppSettings);
			var connection = Environment.Login(loginCredentials);
			ContainerBuilder.RegisterModule<powerVault.Cmdlets.Modules.VaultModule>();
			ContainerBuilder.RegisterInstance(connection.GetServices()).As<coolOrange.VaultServices.Vault.IVaultServices>();
			ContainerBuilder.RegisterModule<Infrastructure>();
			ContainerBuilder.RegisterModule<ServiceMethods>();

			AddServiceMethods();
		}

		class ServiceMethods : Module
		{
			protected override void Load(ContainerBuilder builder)
			{
				builder.RegisterType<Files>().As<ServiceMethod<File>>().As<IServiceMethod>().PropertiesAutowired();
				builder.RegisterType<Properties>().As<ServiceMethod<Property>>().As<IServiceMethod>().PropertiesAutowired();
				builder.RegisterType<Links>().As<ServiceMethod<Link>>().As<IServiceMethod>().PropertiesAutowired();
			}
		}

		static Configuration GetConfiguration()
		{
			var currentAssembly = Assembly.GetCallingAssembly();
			return ConfigurationManager.OpenExeConfiguration(currentAssembly.Location);
		}
	}
}