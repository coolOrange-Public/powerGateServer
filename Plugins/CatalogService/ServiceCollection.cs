using System.Collections.Generic;
using System.Linq;
using CatalogService.Entities;
using powerGateServer.SDK;

namespace CatalogService
{
	public class ServiceCollection : ServiceMethod<Service>
	{
		private readonly WebService _catalogService;

		public ServiceCollection(WebService catalogService)
		{
			_catalogService = catalogService;
		}

		public override IEnumerable<Service> Query(IExpression<Service> expression)
		{
			var webServices = _catalogService.Manager.Services;
			return webServices.Select(webService => new Service(webService));
		}

		public override void Update(Service entity)
		{
		}

		public override void Create(Service entity)
		{
		}

		public override void Delete(Service entity)
		{
		}
	}
}