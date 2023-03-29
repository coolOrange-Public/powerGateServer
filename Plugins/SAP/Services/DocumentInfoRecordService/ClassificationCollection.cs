using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
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