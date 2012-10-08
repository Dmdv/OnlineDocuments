namespace OnlineEditor
{
	/// <summary>
	/// Documents factory, creates, open and deletes documents on a physical layer.
	/// </summary>
	public interface IDocumentsFactory
	{
		Document Create(string name);
		Document Open(string name);
		bool Delete(Document doc);
		bool Close(string name);
		bool CanOpen();
		bool CanDelete();
		bool CanClose();
	}
}