using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class DocumentInfoRecordDataCollection : DirNavigationPropertyEntitySet<DocumentInfoRecordData>
	{
		public DocumentInfoRecordDataCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "DocumentInfoRecordDataCollection"; }
		}
	}
}