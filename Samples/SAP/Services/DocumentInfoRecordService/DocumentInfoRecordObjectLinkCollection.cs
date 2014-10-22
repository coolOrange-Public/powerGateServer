using UserServices.Entities;

namespace UserServices.ServiceDefinition
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