using System.Collections.Generic;
using Autofac;
using powerGateServer.SDK;

namespace VaultServices
{
	public abstract class ContainerBasedService : WebService
	{
		protected ContainerBuilder ContainerBuilder = new ContainerBuilder();

		protected void AddServiceMethods()
		{
			var container = ContainerBuilder.Build();
			foreach (var serviceMethod in container.Resolve<IEnumerable<IServiceMethod>>())
				AddMethod(serviceMethod);
		}
	}
}