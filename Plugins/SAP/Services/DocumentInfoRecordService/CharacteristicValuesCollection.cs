using SapServices.Database;
using SapServices.Services.DocumentInfoRecordService.Entities;

namespace SapServices.Services.DocumentInfoRecordService
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