using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
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