using System;
using System.IO;
using System.Text;

namespace OnlineEditor.Data
{
	/// <summary>
	/// Document.
	/// </summary>
	internal class Document : IDisposable
	{
		private readonly Stream _stream;

		public Document(Stream stream, string name)
		{
			_stream = stream;
			IsOpened = false;
			Name = name;
			Encoding = Encoding.UTF8;
		}

		public Encoding Encoding { get; private set; }

		/// <summary>
		/// Имя документа.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Документ открыт.
		/// </summary>
		public bool IsOpened { get; set; }

		/// <summary>
		/// Владелец документа на текущий момент.
		/// </summary>
		public string Owner { get; set; }

		/// <summary>
		/// Создатель документа.
		/// </summary>
		public string Creator { get; set; }

		/// <summary>
		/// Время создания документа.
		/// </summary>
		public DateTime CreationTime { get; set; }

		public void Write(string text)
		{
			_stream.Seek(0, SeekOrigin.Begin);
			var bytes = Encoding.GetBytes(text);
			_stream.Write(bytes, 0, bytes.Length);
		}

		public string Read()
		{
			_stream.Seek(0, SeekOrigin.Begin);
			var buffer = new byte[_stream.Length];
			_stream.Read(buffer, 0, buffer.Length);
			return Encoding.GetString(buffer);
		}

		/// <summary>
		/// Dispose.
		/// </summary>
		public void Dispose()
		{
			_stream.Close();
			_stream.Dispose();
			IsOpened = false;
		}
	}
}