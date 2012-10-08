using System;
using OnlineEditor.Extensions;
using System.Collections.Generic;
using System.Linq;
using OnlineEditor.Managers;
using OnlineEditor.Service;

namespace OnlineEditor
{
	/// <summary>
	/// Documents container, tracks currently available documents.
	/// </summary>
	internal class DocumentsContainer : IDisposable
	{
		private readonly IDocumentsFactory _factory;
		private readonly Dictionary<string, Document> _hash;
		private InternalResult _lastResult = new InternalResult();
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
					pair.Value.Dispose();
				}
			}
		}

		public Result Create(string name, string owner)
		{
			lock (_sync)
			{
				name = UniqueName(name);
				var document = Invoke.Safe(() => _factory.Create(name), LastError);
				if (document == null)
				{
					return new Result {State = State.Deleted, Success = false, Buffer = _lastResult.ToString()};
				}

				document.Creator = owner;
				document.CreationTime = DateTime.Now;
				_hash[name] = document;
				return new Result {State = State.Created, Success = true};
			}
		}

		public Result Open(string name, string user)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return new Result { State = State.Deleted, Success = false };
				if (IsOpened(name) && !IsOwner(name, user)) return new Result { State = State.Opened, Success = false };

				var document = _hash[name];
				var text = Invoke.Safe(() => _factory.Read(document), LastError);

				if (_lastResult.Success)
				{
					document.IsOpened = true;
					document.Owner = user;
				}

				return
					_lastResult.Success
						? new Result {Success = true, Buffer = text, State = State.Opened}
						: new Result {Success = false, Buffer = _lastResult.ToString()};
			}
		}

		public Result Write(string name, string user, string text)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return new Result { State = State.Deleted, Success = false };
				if (IsOpened(name) && !IsOwner(name, user)) return new Result { State = State.Opened, Success = false };

				var document = _hash[name];
				var result = Invoke.Safe(() => _factory.Write(document, text), LastError);
				return new Result {State = State.Opened, Success = result};
			}
		}

		/// <summary>
		/// Closing without deleting.
		/// User informs that he won't edit the text.
		/// </summary>
		public Result Close(string name, string user)
		{
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return new Result {State = State.Deleted, Success = false};
				if (IsOpened(name) && !IsOwner(name, user)) return new Result {State = State.Opened, Success = false};

				var document = _hash[name];

				if (_factory.SupportClose())
					Invoke.Safe(() => _factory.Close(document), LastError);

				document.IsOpened = false;
				document.Owner = string.Empty;
			}

			return new Result {State = State.Closed, Success = true};
		}

		/// <summary>
		/// Closing and deleting.
		/// </summary>
		public Result Delete(string name, string user)
		{
			var result = Close(name, user);
			if (!result.Success) return result;

			var deleted = true;
			lock (_sync)
			{
				if (!_hash.ContainsKey(name)) return new Result { State = State.Deleted, Success = false };
				if (IsOpened(name) && !IsOwner(name, user)) return new Result {State = State.Opened, Success = false};

				if (_factory.SupportDelete())
					deleted = Invoke.Safe(() => _factory.Delete(_hash[name]), LastError);

				if (deleted)
					_hash.Remove(name);
			}

			return new Result {State = deleted ? State.Deleted : State.Closed, Success = deleted};
		}

		public string[] AvailableDocuments()
		{
			lock (_sync)
			{
				return _hash
					.OrderBy(x=>x.Value.CreationTime)
					.Select(x => x.Value.IsOpened ? string.Format("{0} (read only: {1})", x.Key, x.Value.Owner) : x.Key)
					.ToArray();
			}
		}

		private string UniqueName(string name)
		{
			var temp = name;
			while (_hash.ContainsKey(temp))
			{
				temp = string.Format("{0} {1}", name, new Random().Next(10000));
			}
			return temp;
		}

		private void LastError(Exception ex)
		{
			_lastResult = new InternalResult {Exception = ex, Success = ex == null};
		}

		private bool IsOpened(string name)
		{
			return _hash[name].IsOpened;
		}

		private bool IsOwner(string name, string owner)
		{
			return _hash[name].Owner == owner;
		}
	}
}