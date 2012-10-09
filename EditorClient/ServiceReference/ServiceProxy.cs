using System;
using System.ServiceModel;
using OnlineEditor.Service;

namespace EditorClient.ServiceReference
{
	public static class Service
	{
		public static void Start()
		{
			try
			{
				Logger = new DefaultLogger();
				var factory = new ChannelFactory<IDocumentsRepository>(ServiceFactory.Binding,
				                                                       new EndpointAddress(ServiceFactory.Uri));
				Proxy = factory.CreateChannel();
				Proxy.Ping();
			}
			catch (Exception)
			{
				Logger.LogError("Service is not available");
				Proxy = null;
			}
		}

		public static IDocumentsRepository Proxy { get; set; }

		public static bool Online
		{
			get { return Proxy != null; }
		}

		public static string CurrentUser
		{
			get { return Environment.UserName; }
		}

		public static ILogger Logger { get; set; }
	}
}