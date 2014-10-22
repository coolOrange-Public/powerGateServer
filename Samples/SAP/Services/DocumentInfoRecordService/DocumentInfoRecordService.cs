using UserServices.Entities;
using UserServices.ServiceDefinition;

namespace UserServices.Services.DocumentInfoRecordService
{
	public class DocumentInfoRecordService : SapServiceBase
	{
		public override string Name
		{
			get { return "Arcona6/DOCUMENT_INFO_RECORD_SRV"; }
		}

		protected override void CreateTablesForEntities()
		{
			EntityStores.AddStoreFor<DocumentInfoRecordData>();
			EntityStores.AddStoreFor<DocumentInfoRecordOriginal>();
			EntityStores.AddStoreFor<DocumentInfoRecordObjectLink>();
			EntityStores.AddStoreFor<CharacteristicValues>();
			EntityStores.AddStoreFor<Classification>();
			EntityStores.AddStoreFor<DocumentInfoRecordDescription>();
			EntityStores.AddStoreFor<DocumentInfoRecordContext>();
		}


		protected override void RegisterEntitySets()
		{
			AddEntitySetOfType<DocumentInfoRecordDataCollection>();
			AddEntitySetOfType<DocumentInfoRecordOriginalCollection>();
			AddEntitySetOfType<CharacteristicValuesCollection>();
			AddEntitySetOfType<ClassificationCollection>();
			AddEntitySetOfType<DocumentInfoRecordObjectLinkCollection>();
			AddEntitySetOfType<DocumentInfoRecordDescriptionCollection>();
			AddEntitySetOfType<DocumentInfoRecordContextCollection>();
		}

		protected override void RegisterLookups()
		{
		}
	}
}
