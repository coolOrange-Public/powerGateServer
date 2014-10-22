using System.Collections.Generic;
using powerGateServer.Addins;
using powerGateServer.Addins.WCFspecific;

namespace UserServices.ServiceDefinition
{
	public class CatalogService : IWebService
	{
		public string Name { get { return "IWFND/CATALOGSERVICE"; } }
		public IEnumerable<IServiceMethod> Methods { get; private set; }

		public CatalogService()
		{
			var webServicesInfo = new WebServicesInfo();
			Methods = new IServiceMethod[]
			{
				new ServiceCollection(webServicesInfo.GetAllWebserviceTypes()), 
				new CatalogCollection()
			};
		}
	}
}
