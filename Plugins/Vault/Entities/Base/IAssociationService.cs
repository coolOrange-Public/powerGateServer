using System.Collections.Generic;

namespace VaultServices.Entities.Base
{
	public interface IAssociationService<in T> where T : IBaseObject
	{
		IEnumerable<Link.Link> GetParents(T entity);
		IEnumerable<Link.Link> GetChildren(T entity);
	}
}