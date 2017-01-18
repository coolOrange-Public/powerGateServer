using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
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