using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class CharacteristicValuesCollection : DirNavigationPropertyCollectionEntitySet<CharacteristicValues>
	{
		public CharacteristicValuesCollection(IEntityStores entityStores)
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "CharacteristicValuesCollection"; }
		}
	}
}