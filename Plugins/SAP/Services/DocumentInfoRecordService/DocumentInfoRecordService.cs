using powerGateServer.SDK;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
{
	[WebServiceData("sap/opu/odata/Arcona6", "DOCUMENT_INFO_RECORD_SRV")]
	public class DocumentInfoRecordService : SapServiceBase
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
