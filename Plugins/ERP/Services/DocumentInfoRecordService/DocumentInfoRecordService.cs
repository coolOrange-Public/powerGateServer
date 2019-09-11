using ErpServices.Services.DocumentInfoRecordService.Entities;
using powerGateServer.SDK;

namespace ErpServices.Services.DocumentInfoRecordService
{
	[WebServiceData("PGS/ERP", "DOCUMENT_INFO_RECORD_SRV")]
	public class DocumentInfoRecordService : ErpServiceBase
	{
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
