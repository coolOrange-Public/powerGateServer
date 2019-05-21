using System.Configuration;

namespace VaultServices.Environment
{
	public struct VaultLoginCredentials
	{
		public string Password { get; set; }
		public string Server { get; set; }
		public string User { get; set; }
		public string Vault { get; set; }

		public VaultLoginCredentials(AppSettingsSection configuration)
			: this()
		{
			var settings = configuration.Settings;
			User = settings["User"].Value;
			Password = settings["Password"].Value;
			Server = settings["Server"].Value;
			Vault = settings["Vault"].Value;
		}
	}
}