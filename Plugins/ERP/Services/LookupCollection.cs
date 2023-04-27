using System;
using ErpServices.Database;

namespace ErpServices.Services
{
	public class LookupCollection<T> : ErpServiceMethodBase<T>
	{
		public LookupCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		protected override string ClientId
		{
			get { return string.Empty; }
		}

		public override void Create(T entity)
		{
			ThrowCollectionIsReadOnly(entity);
		}

		public override void Update(T entity)
		{
			ThrowCollectionIsReadOnly(entity);
		}

		public override void Delete(T entity)
		{
			ThrowCollectionIsReadOnly(entity);
		}

		public override string Name
		{
			get { return string.Format("{0}sLookup", typeof(T).Name.Replace("Lookup", string.Empty)); }
		}
		
		void ThrowCollectionIsReadOnly(T entity)
		{
			Throw(entity,"EntitySet '{0}' can not be modified, because it is read-only!");
		}
	}
}