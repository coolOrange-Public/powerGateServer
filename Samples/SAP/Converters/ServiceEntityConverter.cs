using System;
using powerGateServer.Addins;
using powerGateServer.Server;
using UserServices.Entities;

namespace UserServices.Converters
{
	public class ServiceEntityConverter : ITypeConverter<Service,Type>
	{
		public Service ConvertFrom(Type to)
		{
			var webSvc = CreateWebservice(to);
			var webSvcInfo = new ServiceUri(webSvc.Name);

			return new Service
			{
				Description = webSvcInfo.ServiceName,
				Title = webSvcInfo.ServiceName,
				Author = "coolOrange",
				TechnicalServiceVersion = 1,
				ID = string.Format("/{0}/{1}_0001",
						webSvcInfo.BundleName, webSvcInfo.ServiceName),
				MetadataUrl = string.Format("{0}/$metadata", webSvcInfo.Url),
				TechnicalServiceName = string.Format("/{0}/{1}", 
						webSvcInfo.BundleName, webSvcInfo.ServiceName),
				ServiceUrl = webSvcInfo.Url,
				ImageUrl = "",
				UpdatedDate = DateTime.Now
			};
		}

		public Type ConvertTo(Service @from)
		{
			throw new NotImplementedException();
		}

		private IWebService CreateWebservice(Type type)
		{
			if (!typeof(IWebService).IsAssignableFrom(type))
				throw new Exception(
					string.Format("Type '{0}' is not derived from interface '{1}'",
					type.Name, typeof(IWebService).Name));

			try
			{
				return (IWebService)Activator.CreateInstance(type);
			}
			catch (Exception)
			{
				throw new Exception(
					string.Format(
						"Failed to create a new Webservice of type: {0}! " +
						"Are you shure that this class has a public default-constructor?",
					type.Name));
			}
		}
	}
}
