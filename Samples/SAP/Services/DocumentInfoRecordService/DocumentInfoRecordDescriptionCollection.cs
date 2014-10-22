using UserServices.Entities;

namespace UserServices.ServiceDefinition
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