using powerGateServer.SDK;

namespace CatalogService
{
	[WebServiceData("IWFND", "CATALOGSERVICE")]
	public class CatalogService : WebService
	{
		public CatalogService()
		{
			AddMethod(new ServiceCollection(this));
			AddMethod(new CatalogCollection());
		}
	}
}
