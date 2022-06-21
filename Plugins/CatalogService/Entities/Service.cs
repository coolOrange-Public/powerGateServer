using System;
using System.Data.Services.Common;
using powerGateServer.SDK;

namespace CatalogService.Entities
{
	[DataServiceKey("ID")]
	public class Service
	{
		public string ID { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int TechnicalServiceVersion { get; set; }
		public string MetadataUrl { get; set; }
		public string TechnicalServiceName { get; set; }
		public string ImageUrl { get; set; }
		public string ServiceUrl { get; set; }
		public DateTime UpdatedDate { get; set; }

		public Service()
		{
		}

		public Service(WebService webService)
		{
			var serviceInfo = webService.Info;
			Title = Description = serviceInfo.ServiceName;

			var pluginAssembly = webService.GetType().Assembly;
			var pluginInfo = new PluginInfo(pluginAssembly);

			Author = pluginInfo.GetCompany();
			TechnicalServiceVersion = 1;

			ID = string.Format("/{0}/{1}_0001",
				serviceInfo.Path, serviceInfo.ServiceName);
			MetadataUrl = string.Format("{0}/$metadata", serviceInfo.Url);
			TechnicalServiceName = string.Format("/{0}/{1}",
				serviceInfo.Path, serviceInfo.ServiceName);
			ServiceUrl = serviceInfo.Url.ToString();
			ImageUrl = "";
			UpdatedDate = pluginInfo.GetBuildTime();
		}
	}
}