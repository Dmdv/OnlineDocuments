using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OnlineEditor.Service;

namespace EditorServiceHost
{
	static class Program
	{
		static void Main()
		{
			var serviceHost = new ServiceHost(typeof (DocumentsRepository), new Uri(ServiceFactory.Uri));
			var endpoint = serviceHost.AddServiceEndpoint(typeof (IDocumentsRepository), ServiceFactory.Binding, string.Empty);
			endpoint.Name = ServiceFactory.Name;
			var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
			serviceHost.Description.Behaviors.Add(behavior);
			serviceHost.AddServiceEndpoint(typeof (IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
			serviceHost.Open();

			Console.WriteLine("Press <ENTER> to exit...");
			Console.ReadLine();
			serviceHost.Close();
		}
	}
}