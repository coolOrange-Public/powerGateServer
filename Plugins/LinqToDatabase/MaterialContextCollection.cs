using IQToolkit.Data;
using powerGateServer.Addins;
using UserServices.LinqToDatabase.Entities;

namespace UserServices.LinqToDatabase
{
	public class MaterialContextCollection : EntityProviderServiceMethod<MaterialContext>
	{
		public MaterialContextCollection(DbEntityProvider entityProvider) 
			: base(entityProvider)
		{
		}

		public override string Name
		{
			get { return "MaterialContextCollection"; }
		}
	}
}