using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
{

	public class DocumentInfoRecordContextCollection : ContextEntitySetBase<DocumentInfoRecordContext, DocumentInfoRecordContext>
	{
		public DocumentInfoRecordContextCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "DocumentInfoRecordContextCollection"; }
		}

		public override void Update(DocumentInfoRecordContext entity)
		{
			Throw(entity, "{0} has no property that can be updated because it has only keys! " +
			              "For updating navigation properties please use the correct EntitySet!");
		}
	}
}