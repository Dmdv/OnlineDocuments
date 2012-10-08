using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineEditor
{
	/// <summary>
	/// Documents container, tracks currently available documents.
	/// </summary>
	internal class DocumentsContainer : IDocumentsFactory, IDisposable
	{
		private readonly IDocumentsFactory _factory;
		private readonly Dictionary<string, Document> _hash;
		private readonly object _sync = new object();

		public DocumentsContainer(IDocumentsFactory factory)
		{
			_factory = factory;
			_hash = new Dictionary<string, Document>();
		}

		public void Dispose()
		{
			lock (_sync)
			{
				foreach (var pair in _hash.Where(pair => pair.Value.IsOpened))
				{
					pair.Value.Close();
				}
			}
		}

		public Document Create(string name)
		{
			Document document = null;

			lock (_sync)
			{
				var temp = name;
				while (_hash.ContainsKey(temp))
				{
					temp = string.Format("{0} {1}", name, new Random().Next(10000));
				}
				
				try
				{
					document = _factory.Create(temp);
				}
				catch (Exception)
				{
					return document;
				}

				_hash[temp] = document;
			}
			
			return document;
		}

		public Document Open(string name)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return null;
				if (IsOpened(name)) return null;
				var document = _hash[name];
				document.IsOpened = true;
				return _factory.CanOpen() ? _factory.Open(name) : document;
			}
		}

		public bool Delete(string name)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return false;
				if (IsOpened(name)) return false;
				var document = _hash[name];
				document.Close();
				if (_factory.CanDelete())
					_factory.Delete(document);
				_hash.Remove(name);
			}
			return true;
		}

		public bool Delete(Document doc)
		{
			return Delete(doc.Name);
		}

		public bool Close(string name)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return false;
				var document = _hash[name];
				if (IsOpened(document))
				{
					// DO NOT CLOSE MANUALLY!
					if (_factory.CanClose())
						_factory.Close(name);
					document.IsOpened = false;
				}
			}
			return true;
		}

		public bool CanOpen()
		{
			return _factory.CanOpen();
		}

		public bool CanDelete()
		{
			return _factory.CanDelete();
		}

		public bool CanClose()
		{
			return _factory.CanClose();
		}

		private bool IsOpened(Document document)
		{
			return document == null || document.IsOpened;
		}

		private bool IsOpened(string name)
		{
			return IsOpened(_hash[name]);
		}
	}
}