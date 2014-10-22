using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;
using powerGateServer.Addins.Extensions;
using UserServices.Vault.Entities;
using UserServices.Vault.FindStrategies;

namespace UserServices.Vault
{
	public class DocumentService : ServiceMethod<File>
	{
		public override string Name { get { return "Documents"; } }

		private readonly IVaultConnection _vaultConnection;
		readonly IEntityConverter _entityConverter;
		private readonly IEnumerable<FindVaultFiles> _findVaultFileStrategies;

		public DocumentService(IVaultConnection vaultConnection, IEntityConverter entityConverter)
		{
			_vaultConnection = vaultConnection;
			_entityConverter = entityConverter;

			_findVaultFileStrategies = new FindVaultFiles[]
			{
				new FindVaultFilesByIds(vaultConnection),
				new FindVaultFilesByMasterIds(vaultConnection),
				new FindVaultFilesByProperties(vaultConnection, _entityConverter)
			};
		}

		public override IEnumerable<File> Query(IExpression<File> query)
		{
			var vaultFiles = new List<Autodesk.Connectivity.WebServices.File>();
			
			var findFileStrategies = _findVaultFileStrategies.Where(findStrategy => findStrategy.CanFind(query));
			foreach (var findFiles in findFileStrategies)
				vaultFiles.AddRange(findFiles.Find());
			return vaultFiles.DistinctBy(f => f.Id)
				.Select(f => _entityConverter.ToDataServiceFile(f));
		}

		public override void Update(File entity)
		{
			var originalVaultFile = _vaultConnection.WebServiceManager.DocumentService.GetFileById(entity.Id);
			var original = _entityConverter.ToDataServiceFile(originalVaultFile);
			if (entity.Category != original.Category)
				_vaultConnection.UpdateFileCategory(originalVaultFile,entity.Category);
		}

		public override void Create(File entity)
		{
		}

		public override void Delete(File entity)
		{
		}
	}
}
