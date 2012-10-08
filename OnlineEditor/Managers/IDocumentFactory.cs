namespace OnlineEditor.Managers
{
	/// <summary>
	/// Documents factory, creates, open and deletes documents on a physical layer.
	/// </summary>
	internal interface IDocumentsFactory
	{
		Document Create(string name);
		string Read(Document doc);
		bool Write(Document doc, string text);
		bool Close(Document doc);
		bool Delete(Document doc);

		bool SupportWriting();
		bool SupportDelete();
		bool SupportClose();
	}
}