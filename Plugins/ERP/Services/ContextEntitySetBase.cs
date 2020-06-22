using ErpServices.Database;

namespace ErpServices.Services
{
	public abstract class ContextEntitySetBase<T,TContext> : ErpServiceMethodBase<T>
	{
		protected ContextEntitySetBase(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		protected IEntityStore<TContext> ContextStore
		{
			get { return EntityStores.ResolveFor<TContext>(); }
		}
	}
}