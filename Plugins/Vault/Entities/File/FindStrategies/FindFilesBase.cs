using System.Collections.Generic;
using System.Linq;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using powerVault.Cmdlets.Cmdlets.Vault.Facade.Files;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Entities.File.FindStrategies
{
	public abstract class FindFilesBase : IQueryOperation<File>
	{
		private readonly IVaultEntityFactory _entityFactory;
		private readonly IAssociationService<File> _associationService;
		private IExpression<File> _query;

		protected FindFilesBase(FileConversionContext conversionContext)
		{
			_entityFactory = conversionContext.EntityFactory;
			_associationService = conversionContext.AssociationService;
		}

		protected abstract IEnumerable<Vault.File> Execute();
		protected abstract bool CanExecute(IExpression<File> expression);

		bool IQueryOperation<File>.CanExecute(IExpression<File> expression)
		{
			_query = expression;
			return CanExecute(expression);
		}

		IEnumerable<File> IQueryOperation<File>.Execute()
		{
			var vaultFiles = Execute().Select(file => _entityFactory.Create(file));
			_query.Expand.OnNavigationPropertyDemand(file => file.Properties).Call(
				file => ConvertProperties(vaultFiles, file));
			_query.Expand.OnNavigationPropertyDemand(file => file.Children).Call(
				file => _associationService.GetChildren(file));
			_query.Expand.OnNavigationPropertyDemand(file => file.Parents).Call(
				file => _associationService.GetParents(file));
			return vaultFiles.Select(vF => new File(vF.Base)).ToList();
		}

		private IEnumerable<Property.Property> ConvertProperties(IEnumerable<VaultFile> vaultFiles, File file)
		{
			var vaultFile = vaultFiles.FirstOrDefault(f => f.Identifiers.Id == file.Id);
			return ((IEnumerable<IProperty>)vaultFile.Properties).Select(vProp => new Property.Property(file, vProp));
		}
	}
}