using System;
using System.Collections.Generic;
using System.Linq;
using powerGateServer.Addins;
using UserServices.Converters;
using UserServices.Entities;

namespace UserServices.ServiceDefinition
{
	public class ServiceCollection : ReadonlyServiceMethod<Service>
	{
		public override string Name
		{
			get { return "ServiceCollection"; }
		}
		private readonly ITypeConverter<Service, Type> _serviceEntiTypeConverter = new ServiceEntityConverter();
		private readonly IEnumerable<Type> _webserviceTypes;
		
		public ServiceCollection(IEnumerable<Type> webserviceTypes)
		{
			_webserviceTypes = webserviceTypes;
		}

		public override IEnumerable<Service> Query(IExpression<Service> expression)
		{
			return _webserviceTypes.Select(t => _serviceEntiTypeConverter.ConvertFrom(t));
		}
	}
}