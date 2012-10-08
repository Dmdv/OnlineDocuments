using System;

namespace OnlineEditor.Managers
{
	/// <summary>
	/// File system document implemenantation.
	/// </summary>
	internal class FileSystemDocuments : IDocumentsFactory
	{
		public Document Create(string name)
		{
			throw new NotImplementedException();
		}

		public string Read(Document doc)
		{
			throw new NotImplementedException();
		}

		public bool Write(Document doc, string text)
		{
			throw new NotImplementedException();
		}

		public bool Close(Document doc)
		{
			throw new NotImplementedException();
		}

		public Document Open(string name)
		{
			throw new NotImplementedException();
		}

		public bool Delete(Document doc)
		{
			throw new NotImplementedException();
		}

		public bool Delete(string name)
		{
			throw new NotImplementedException();
		}

		public bool Close(string name)
		{
			throw new NotImplementedException();
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
			return true;
		}
	}
}