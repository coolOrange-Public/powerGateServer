using System;
using System.IO;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using powerVault.Services;
using powerVault.Services.Vault;
using VaultServices.Environment;

namespace VaultServices.Tests
{
	[TestFixture]
	public class PowerVaultAssemblyLoaderTests : ContainerBasedTests
	{
		[Test]
		public void AddResolver_adds_assembly_resolver_to_powerVault_installation_directory_found_in_psModulePath_variable()
		{
			Container.SubstituteFor<ISystem>().GetEnvironmentVariable("PSModulePath").Returns(
				@"%SystemRoot%\system32\WindowsPowerShell\v1.0\Modules\;" + AppDomain.CurrentDomain.BaseDirectory+";");

			var powerVault = Container.Resolve<PowerVaultAssemblyLoader>();
			powerVault.AddResolver();

			Container.Resolve<IAssemblyResolver>().Received()
				.ResolveDirectory(AppDomain.CurrentDomain.BaseDirectory+@"\powerVault");
		}

		[Test]
		public void AddResolver_throws_exception_when_powerVault_installation_directory_not_found_in_psModulePath_variable()
		{
			Container.SubstituteFor<ISystem>().GetEnvironmentVariable("PSModulePath").Returns(
				@"%SystemRoot%\system32\WindowsPowerShell\v1.0\Modules\;C:\Program Files\plaplapla;");

			var powerVault = Container.Resolve<PowerVaultAssemblyLoader>();
			Assert.Throws<ApplicationException>(() => powerVault.AddResolver());
		}

		[Test]
		public void GetDirectory_finds_the_directory_by_checking_all_the_moduleDirectories_from_psModulePath_if_it_contains_powerVault_module()
		{
			var currentAssemblyDir = AppDomain.CurrentDomain.BaseDirectory;
			Container.SubstituteFor<ISystem>().GetEnvironmentVariable("PSModulePath").Returns(
				@"%SystemRoot%\system32\WindowsPowerShell\v1.0\Modules\;" + currentAssemblyDir
				+ @";C:\Program Files\coolOrange\powerVault;C:\Program Files\coolOrange\Modules;");

			var powerVault = Container.Resolve<PowerVaultAssemblyLoader>();
			Assert.AreEqual(currentAssemblyDir + @"\powerVault", powerVault.GetDirectory());
		}
	}
}
