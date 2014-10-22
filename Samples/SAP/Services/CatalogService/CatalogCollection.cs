using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;
using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class CatalogCollection : ReadonlyServiceMethod<Catalog>
	{
		public override string Name
		{
			get { return "CatalogCollection"; }
		}

		public override IEnumerable<Catalog> Query(IExpression<Catalog> expression)
		{
			return Enumerable.Empty<Catalog>();
		}
	}
}