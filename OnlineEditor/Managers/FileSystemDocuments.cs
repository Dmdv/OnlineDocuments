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

		public bool CanOpen()
		{
			return true;
		}

		public bool CanDelete()
		{
			return true;
		}

		public bool CanClose()
		{
			return true;
		}
	}
}