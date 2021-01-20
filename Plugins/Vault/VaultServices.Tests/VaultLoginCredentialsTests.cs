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
			settings.Settings.Add("User","Barack");
			settings.Settings.Add("Password", "I<3Angela4Ever");
			settings.Settings.Add("Server", "NSA");
			settings.Settings.Add("Vault", "TopSecretData");

			var credentials = new VaultLoginCredentials(settings);

			Assert.AreEqual("Barack", credentials.User);
			Assert.AreEqual("I<3Angela4Ever", credentials.Password);
			Assert.AreEqual("NSA", credentials.Server);
			Assert.AreEqual("TopSecretData", credentials.Vault);
		}
	}
}