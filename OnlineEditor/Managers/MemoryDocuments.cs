using System.IO;
using OnlineEditor.Data;

namespace OnlineEditor.Managers
{
	/// <summary>
	/// Factory for documents kept in memory.
	/// </summary>
	internal class MemoryDocuments : IDocumentsFactory
	{
		public Document Create(string name)
		{
			return new Document(new MemoryStream(), name);
		}

		public string Read(Document doc)
		{
			return doc.Read();
		}

		public bool Write(Document doc, string text)
		{
			doc.Write(text);
			return true;
		}

		public bool Delete(Document doc)
		{
			doc.Dispose();
			return true;
		}

		public bool Close(Document doc)
		{
			return false;
		}

		public bool SupportWriting()
		{
			return true;
		}

		public bool SupportDelete()
		{
			return true;
		}

		public bool SupportClose()
		{
			return false;
		}
	}
}