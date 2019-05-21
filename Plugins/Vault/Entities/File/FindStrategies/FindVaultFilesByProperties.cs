using System.Collections.Generic;
using Autodesk.Connectivity.WebServices;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.Base.FindStrategies;
using IVaultServices = coolOrange.VaultServices.Vault.IVaultServices;

namespace VaultServices.Entities.File.FindStrategies
{
	public class FindVaultFilesByProperties : FindVaultEntityByProperties<File>
	{
		readonly IQueryOperation<File> _base;

		protected override string EntityType
		{
			get { return "FILE"; }
		}

		public FindVaultFilesByProperties(FileConversionContext fileConversionContext, IVaultServices vaultServices)
			: base(vaultServices)
		{
			_base = new FindFilesByProperties(this, fileConversionContext);
		}

		public override IEnumerable<File> Execute()
		{
			return _base.Execute();
		}

		public override bool CanExecute(IExpression<File> expression)
		{
			return _base.CanExecute(expression) && base.CanExecute(expression);
		}

		class FindFilesByProperties : FindFilesBase
		{
			readonly FindVaultEntityByProperties<File> _findVaultEntityByProperties;

			public FindFilesByProperties(FindVaultEntityByProperties<File> findVaultEntityByProperties, FileConversionContext fileConversionContext)
				: base(fileConversionContext)
			{
				_findVaultEntityByProperties = findVaultEntityByProperties;
			}

			IVaultServices VaultServices { get { return _findVaultEntityByProperties.VaultServices; } }
			IEnumerable<SrchCond> SearchConditions { get { return _findVaultEntityByProperties.SearchConditions; } }
			IEnumerable<SrchSort> SearchSortings { get { return _findVaultEntityByProperties.SearchSortings; } }

			protected override IEnumerable<Autodesk.Connectivity.WebServices.File> Execute()
			{
				return VaultServices.FindFilesBySearchConditions(SearchConditions, SearchSortings, recursive: true);
			}

			protected override bool CanExecute(IExpression<File> expression)
			{
				return true;
			}
		}
    }
}
