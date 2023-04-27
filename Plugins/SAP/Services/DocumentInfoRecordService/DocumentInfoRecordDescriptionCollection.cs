using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
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