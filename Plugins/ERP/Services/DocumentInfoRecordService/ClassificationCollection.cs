using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
{
	public class ClassificationCollection : DirNavigationPropertyCollectionEntitySet<Classification>
	{
		public ClassificationCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "ClassificationCollection"; }
		}
	}
}