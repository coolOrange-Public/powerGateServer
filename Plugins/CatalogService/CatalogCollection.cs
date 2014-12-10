using System.Collections.Generic;
using System.Linq;
using CatalogService.Entities;
using powerGateServer.SDK;

namespace CatalogService
{
	public class CatalogCollection : ServiceMethod<Catalog>
	{
		public override IEnumerable<Catalog> Query(IExpression<Catalog> expression)
		{
			return Enumerable.Empty<Catalog>();
		}

		public override void Update(Catalog entity)
		{
		}

		public override void Create(Catalog entity)
		{
		}

		public override void Delete(Catalog entity)
		{
		}
	}
}