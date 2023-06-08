using System;
using System.ServiceModel.Configuration;

namespace TrafficLogging
{
	public class TrafficLoggingBehaviorExtensionElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get { return typeof(TrafficLoggingBehavior); }
		}

		protected override object CreateBehavior()
		{
			return new TrafficLoggingBehavior();
		}
	}
}
