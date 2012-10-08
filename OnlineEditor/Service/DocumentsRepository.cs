using OnlineEditor.Managers;

namespace OnlineEditor.Service
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

		public Result Create(string name, string owner)
		{
			return _container.Create(name, owner);
		}

		public Result Open(string name, string owner)
		{
			return _container.Open(name, owner);
		}

		public Result Write(string name, string owner, string text)
		{
			return _container.Write(name, owner, text);
		}

		public Result Close(string name, string owner)
		{
			return _container.Close(name, owner);
		}

		public Result Delete(string name, string owner)
		{
			return _container.Delete(name, owner);
		}

		public string[] AvailableDocuments()
		{
			return _container.AvailableDocuments();
		}
	}
}