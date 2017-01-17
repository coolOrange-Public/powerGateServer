using System.Configuration;
using System.Reflection;
using Autodesk.Connectivity.WebServicesTools;
using powerGateServer.SDK;

namespace UserServices.Vault
{
	[WebServiceData("Arcona6", "VAULT_SRV")]
	public class VaultService : WebService
	{
		private readonly KeyValueConfigurationCollection _settings = LoadConfig().AppSettings.Settings;

		public VaultService()
		{
			var credentials = new UserPasswordCredentials(
				_settings["Server"].Value, _settings["Vault"].Value,
				_settings["User"].Value, _settings["Password"].Value);
			var webServiceMgr = new WebServiceManager(credentials);
			var vaultConnection = new VaultConnection(webServiceMgr);
			var entityConverter = new VaultEntityConverter(vaultConnection);
			AddMethod(new DocumentService(vaultConnection, entityConverter));
			AddMethod(new PropertyService());
		}

		public static Configuration LoadConfig()
		{
			Assembly currentAssembly = Assembly.GetCallingAssembly();
			return ConfigurationManager.OpenExeConfiguration(currentAssembly.Location);
		}
	}
}