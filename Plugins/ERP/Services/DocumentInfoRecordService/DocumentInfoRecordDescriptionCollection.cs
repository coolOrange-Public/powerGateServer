using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
{
	public class DocumentInfoRecordDescriptionCollection :
		DirNavigationPropertyCollectionEntitySet<DocumentInfoRecordDescription>
	{
		public DocumentInfoRecordDescriptionCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "DocumentInfoRecordDescriptionCollection"; }
		}
	}
}