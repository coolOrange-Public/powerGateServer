using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;
using UserServices.Vault.Entities;

namespace UserServices.Vault
{
	public class PropertyService : ServiceMethod<Property>
	{
		public override string Name { get { return "Properties"; } }
        
		public override IEnumerable<Property> Query(IExpression<Property> expression)
		{
			return Enumerable.Empty<Property>();
		}

		public override void Update(Property entity)
		{
		}

		public override void Create(Property entity)
		{
		}

		public override void Delete(Property entity)
		{
		}
	}
}