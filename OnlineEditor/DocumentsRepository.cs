using OnlineEditor.Managers;

namespace OnlineEditor
{
	/// <summary>
	/// Responsible for creating of document factory and keeps the thread-safe documents container.
	/// </summary>
	public class DocumentsRepository : IDocumentsRepository
	{
		private readonly DocumentsContainer _container;

		/// <summary>
		/// Suppose for the moment we initialize factory from xml
		/// and factory which is instantiaited is a MemoryFactory.
		/// </summary>
		public DocumentsRepository()
		{
			_container = new DocumentsContainer(new MemoryDocuments());
		}

		public Document Create(string name = "Новый документ")
		{
			return _container.Create(name);
		}

		public Document Open(string name)
		{
			return _container.Open(name);
		}

		public bool Delete(Document doc)
		{
			return _container.Delete(doc);
		}

		public bool Delete(string name)
		{
			return _container.Delete(name);
		}

		public bool Close(string name)
		{
			throw new System.NotImplementedException();
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