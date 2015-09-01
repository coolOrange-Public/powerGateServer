using powerGateServer.SDK;

namespace CatalogService
{
	[WebServiceData("PGS", "CATALOGSERVICE")]
	public class CatalogService : WebService
	{
		public CatalogService()
		{
			AddMethod(new ServiceCollection(this));
		}
	}
}
