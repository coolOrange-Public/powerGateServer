using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using log4net;
using System.Reflection;
using System.Text;

namespace TrafficLogging
{
	public class LoggingMessageInspector : IDispatchMessageInspector
	{
		static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			var msg = CopyMessage(ref request);
			var httRequest = GetHttRequest(msg);
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Request:");
			stringBuilder.AppendFormat("{0} - {1}{2}", httRequest.Method, msg.Properties["Via"] as Uri, Environment.NewLine);
			stringBuilder.AppendLine(httRequest.Headers.ToString());
			stringBuilder.AppendLine(GetMessageContent(msg));
			Log.Info(stringBuilder.ToString());
			return null;
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			var msg = CopyMessage(ref reply);
			var httpResponse = GetHttpResponse(msg);
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Response:");
			stringBuilder.AppendLine(Convert.ToString((int) httpResponse.StatusCode));
			stringBuilder.AppendLine(httpResponse.Headers.ToString());
			stringBuilder.AppendLine(GetMessageContent(msg));
			Log.Info(stringBuilder.ToString());
			
		}

		static Message CopyMessage(ref Message request)
		{
			var buffer = request.CreateBufferedCopy(int.MaxValue);
			request = buffer.CreateMessage();
			var msg = buffer.CreateMessage();
			return msg;
		}


		string GetMessageContent(Message message)
		{
			if (message.IsEmpty)
				return string.Empty;
			try
			{
				var bodyReader = message.GetReaderAtBodyContents();
				bodyReader.ReadStartElement("Binary");
				var input = bodyReader.ReadContentAsBase64();
				return Encoding.UTF8.GetString(input);
			}
			catch (Exception e)
			{
				return "Failed to parse message content: " + e;
			}
		}
		
		HttpRequestMessageProperty GetHttRequest(Message message)
		{
			return message.Properties[HttpRequestMessageProperty.Name] as  HttpRequestMessageProperty;
		}

		HttpResponseMessageProperty GetHttpResponse(Message message)
		{
			return message.Properties[HttpResponseMessageProperty.Name] as  HttpResponseMessageProperty;
		}
	}
}