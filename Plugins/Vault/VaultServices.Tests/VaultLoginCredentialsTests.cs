using System.Configuration;
using NUnit.Framework;
using VaultServices.Environment;

namespace VaultServices.Tests
{
	[TestFixture]
	public class VaultLoginCredentialsTests
	{
		[Test]
		public void Creating_instance_from_configuration_sets_all_the_properties_with_correct_data_from_configuration_settings()
		{
			var settings = new AppSettingsSection();
			settings.Settings.Add("User", "Administrator");
			settings.Settings.Add("Password", "<Password>");
			settings.Settings.Add("Server", "10.1.5.8");
			settings.Settings.Add("Vault", "Vault");

			var credentials = new VaultLoginCredentials(settings);

			Assert.AreEqual("Administrator", credentials.User);
			Assert.AreEqual("<Password>", credentials.Password);
			Assert.AreEqual("10.1.5.8", credentials.Server);
			Assert.AreEqual("Vault", credentials.Vault);
		}
	}
}