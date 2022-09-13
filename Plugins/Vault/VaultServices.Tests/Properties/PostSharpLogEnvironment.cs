using System.Reflection;
using log4net;
using PostSharp.Aspects;
using PostSharp.Patterns.Diagnostics;

namespace coolOrange.Logging
{
	[Log(AttributeExclude = true)]
	public static class PostSharpLogEnvironment
	{
		[ModuleInitializer(0)]
		public static void Initialize()
		{
			var log4NetBackend = new Log4NetBackend();
			log4NetBackend.Options.IncludeExceptionDetails = true;
			log4NetBackend.Options.GetLogger = ts => LogManager.GetLogger(ts.SourceType.Assembly, ts.SourceType);
			LoggingServices.DefaultBackend = log4NetBackend;

			var forcesConfigurignLog4NetEnvironmentWhenAttributesApplied =
				LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}