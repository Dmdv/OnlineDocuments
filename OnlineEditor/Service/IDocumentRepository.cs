using System.ServiceModel;

namespace OnlineEditor.Service
{
	/// <summary>
	/// Documents repository.
	/// </summary>
	[ServiceContract]
	public interface IDocumentsRepository
	{
		[OperationContract]
		Result Create(string name, string owner);

		[OperationContract]
		Result Open(string name, string owner);

		[OperationContract]
		Result Write(string name, string owner, string text);

		[OperationContract]
		Result Close(string name, string owner);

		[OperationContract]
		Result Delete(string name, string owner);

		[OperationContract]
		Document[] AvailableDocuments();

		[OperationContract]
		bool Ping();
	}
}