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
			// 1 way: with ChannelFactory
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

			// 2 way: with ClientBase
			var client = new DocumentsRepositoryClient(ServiceFactory.Binding, new EndpointAddress(ServiceFactory.Uri));
			client.Create("doc", Environment.UserName);
		}
	}
}
