using System;
using System.ServiceModel;
using OnlineEditor.Channel;
using OnlineEditor.Service;

namespace ConsoleClient
{
	/// <summary>
	/// Sample usage.
	/// </summary>
	static class Program
	{
		static void Main()
		{
			var factory = new ChannelFactory<IDocumentsRepository>(ServiceFactory.Binding,
			                                                       new EndpointAddress(ServiceFactory.Uri));
			var repository = factory.CreateChannel();
			try
			{
				repository.Create("name", Environment.UserName);
				repository.AvailableDocuments();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			var client = new DocumentsRepositoryClient(ServiceFactory.Binding, new EndpointAddress(ServiceFactory.Uri));
			client.Create("doc", Environment.UserName);
		}
	}
}
