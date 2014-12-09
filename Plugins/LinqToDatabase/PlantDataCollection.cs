using IQToolkit.Data;
using powerGateServer.Addins;
using UserServices.LinqToDatabase.Entities;

namespace UserServices.LinqToDatabase
{
	public class PlantDataCollection : EntityProviderServiceMethod<PlantData>
	{
		public PlantDataCollection(DbEntityProvider entityProvider) 
			: base(entityProvider)
		{
		}

		public override string Name
		{
			get { return "PlantDataCollection"; }
		}
	}
}