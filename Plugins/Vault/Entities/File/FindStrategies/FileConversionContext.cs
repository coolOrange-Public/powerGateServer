using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using VaultServices.Entities.Base;

namespace VaultServices.Entities.File.FindStrategies
{
	public class FileConversionContext
	{
		 public IVaultEntityFactory EntityFactory { get; set; }
		 public IAssociationService<File> AssociationService { get; set; }
	}
}