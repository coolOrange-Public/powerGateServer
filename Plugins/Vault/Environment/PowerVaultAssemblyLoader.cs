using System;
using System.IO;
using System.Linq;
using powerVault.Services;
using powerVault.Services.Vault;

namespace VaultServices.Environment
{
	public class PowerVaultAssemblyLoader : IAssemblyLoader
	{
		const string PsModulePath = "PSModulePath";
		const string ModuleName = "powerVault";

		readonly IAssemblyResolver _assemblyResolver;
		readonly ISystem _system;

		public PowerVaultAssemblyLoader(IAssemblyResolver assemblyResolver, ISystem system)
		{
			_assemblyResolver = assemblyResolver;
			_system = system;
		}

		public void AddResolver()
		{
			var installationDirectory = GetDirectory();
			_assemblyResolver.ResolveDirectory(installationDirectory);
		}

		public string GetDirectory()
		{
			var modulePaths = _system.GetEnvironmentVariable(PsModulePath)
				.Trim()
				.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

			var powerVaultModules = modulePaths.Select(p => Path.Combine(p, ModuleName))
				.Where(Directory.Exists).ToArray();
			if (powerVaultModules.Any())
				return powerVaultModules.First();
			throw new ApplicationException(ModuleName + " was not detected on this machine!",
				new Exception("Failed to find an entry for " + ModuleName + " in environment variable: " + PsModulePath));
		}
	}
}