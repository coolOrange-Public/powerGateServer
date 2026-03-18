using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
{
	public abstract class DirNavigationPropertyCollectionEntitySet<T> : 
		NavigationPropertyCollectionEntitySet<T, DocumentInfoRecordContext>
	{
		protected DirNavigationPropertyCollectionEntitySet(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		protected override bool IsParentFor(DocumentInfoRecordContext parent, T entity)
		{
			return new DocumentInfoRecordContextComparer(entity)
				.Equals(parent);
		}
	}
}