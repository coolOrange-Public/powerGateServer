using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using VaultServices.Environment;

namespace VaultServices.Tests
{
	[TestFixture]
	public class PowerVaultEnvironmentTests : ContainerBasedTests
	{
		[Test]
		public void Login_adds_assembly_resolver_to_all_assemblyLoaders()
		{
			var powerVaultLoader = Substitute.For<IAssemblyLoader>();
			var vaultExplorerLoader = Substitute.For<IAssemblyLoader>();
			Container.Provide<IEnumerable<IAssemblyLoader>>(new[] {powerVaultLoader, vaultExplorerLoader});
			var powerVault = Container.Resolve<PowerVaultEnvironment>();
			powerVault.Environment.Vault = Substitute.For<powerVault.Cmdlets.Cmdlets.Vault.Facade.IVaultConnection>();

			powerVault.Login(new VaultLoginCredentials());

			powerVaultLoader.Received().AddResolver();
			vaultExplorerLoader.Received().AddResolver();
		}

		[Test]
		public void Login_connects_to_vault_with_login_credentials_and_returns_the_vaultConnection()
		{
			var credentials = new VaultLoginCredentials
			{
				User = "Administrator",
				Password = "<Password>",
				Server = "10.1.5.8",
				Vault = "Vault"
			};
			var vaultConnection = Substitute.For<powerVault.Cmdlets.Cmdlets.Vault.Facade.IVaultConnection>();
			
			var powerVault = Container.Resolve<PowerVaultEnvironment>();
			powerVault.Environment.Vault = vaultConnection;
			var connection = powerVault.Login(credentials);

			vaultConnection.Login("Administrator", "<Password>", "10.1.5.8", "Vault");
			Assert.IsNotNull(connection);
		}

		[Test]
		public void Login_throws_exception_when_connecting_server_fails()
		{
			var vaultConnection = Substitute.For<powerVault.Cmdlets.Cmdlets.Vault.Facade.IVaultConnection>();
			vaultConnection.When(v => v.Login(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())).Do(
			info =>
			{
				throw new Exception("Failed to login!");
			});
			var powerVault = Container.Resolve<PowerVaultEnvironment>();
			powerVault.Environment.Vault = vaultConnection;
			Assert.Throws<Exception>(() => powerVault.Login(new VaultLoginCredentials()));
		}
	}
}