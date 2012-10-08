using System;
using System.IO;

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

		public Document Open(string name)
		{
			return null;
		}

		public bool Delete(Document doc)
		{
			try
			{
				doc.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Delete(string name)
		{
			return false;
		}

		public bool Close(string name)
		{
			return false;
		}

		public bool CanOpen()
		{
			return false;
		}

		public bool CanDelete()
		{
			return false;
		}

		public bool CanClose()
		{
			return false;
		}
	}
}