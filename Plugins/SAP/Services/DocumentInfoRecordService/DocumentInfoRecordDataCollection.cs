using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
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