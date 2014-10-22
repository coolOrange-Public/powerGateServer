using System.Collections.Generic;
using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class DescriptionCollection : NavigationPropertyCollectionEntitySet<Description, MaterialContext>
	{
		public override string Name
		{
			get { return "DescriptionCollection"; }
		}


		public DescriptionCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		protected override bool IsParentFor(MaterialContext parent, Description entity)
		{
			return new MaterialContextComparer(entity)
				.Equals(parent);
		}
	}
}