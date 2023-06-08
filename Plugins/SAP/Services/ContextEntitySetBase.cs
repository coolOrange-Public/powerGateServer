using SapServices.Database;

namespace SapServices.Services
{
	public abstract class ContextEntitySetBase<T,TContext> : SapServiceMethodBase<T>
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