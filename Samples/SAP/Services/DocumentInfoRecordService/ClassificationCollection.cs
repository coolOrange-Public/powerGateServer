using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class ClassificationCollection : DirNavigationPropertyCollectionEntitySet<Classification>
	{
		public ClassificationCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "ClassificationCollection"; }
		}
	}
}