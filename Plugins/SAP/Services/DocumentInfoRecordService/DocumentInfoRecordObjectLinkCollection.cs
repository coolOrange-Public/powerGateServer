using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
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