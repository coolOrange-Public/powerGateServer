using SapServices.Database;

namespace SapServices.Services
{
	public class LookupCollection<T> : SapServiceMethodBase<T>
	{
		public LookupCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return string.Format("{0}Collection",typeof(T).Name); }
		}

		public override void Create(T entity)
		{
			if (GetCurrentEntity(entity) != null)
				ThrowElementAllreadyExists(entity);
			EntityStore.Insert(entity);
		}

		public override void Delete(T entity)
		{
			var currentEntity = GetCurrentEntity(entity);
			if (currentEntity == null)
				ThrowElementDoesNotExists(entity);
			EntityStore.Delete(currentEntity);
		}

		public override void Update(T entity)
		{
			Delete(entity);
			Create(entity);
		}
	}
}