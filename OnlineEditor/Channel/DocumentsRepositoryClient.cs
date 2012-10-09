using System.ServiceModel;
using System.ServiceModel.Channels;
using OnlineEditor.Service;

namespace OnlineEditor.Channel
{
	/// <summary>
	/// Documents repository client.
	/// </summary>
	public class DocumentsRepositoryClient : ClientBase<IDocumentsRepository>, IDocumentsRepository
	{
		public DocumentsRepositoryClient(Binding binding, EndpointAddress address) : 
			base(binding, address)
		{
		}

		public Result Create(string name, string owner)
		{
			return Channel.Create(name, owner);
		}

		public Result Open(string name, string owner)
		{
			return Channel.Open(name, owner);
		}

		public Result Write(string name, string owner, string text)
		{
			return Channel.Write(name, owner, text);
		}

		public Result Close(string name, string owner)
		{
			return Channel.Close(name, owner);
		}

		public Result Delete(string name, string owner)
		{
			return Channel.Delete(name, owner);
		}

		public Document[] AvailableDocuments()
		{
			return Channel.AvailableDocuments();
		}

		public bool Ping()
		{
			return Channel.Ping();
		}
	}
}
