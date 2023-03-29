using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TrafficLogging
{
	public class TrafficLoggingBehavior : IServiceBehavior
	{
		public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
		{
			foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.Cast<ChannelDispatcher>())
				foreach (var endpointDispatcher in channelDispatcher.Endpoints)
					endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new LoggingMessageInspector());
		}

		public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
		{
		}
	}
}