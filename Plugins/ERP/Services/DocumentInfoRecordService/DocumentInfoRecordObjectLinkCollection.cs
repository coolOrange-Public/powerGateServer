using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
{
	public class DocumentInfoRecordObjectLinkCollection : DirNavigationPropertyCollectionEntitySet<DocumentInfoRecordObjectLink>
	{
		public override string Name
		{
			get { return "DocumentInfoRecordObjectLinkCollection"; }
		}


		public DocumentInfoRecordObjectLinkCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}
	}
}