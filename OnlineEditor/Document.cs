using System.IO;

namespace OnlineEditor
{
	public class Document : StreamWriter
	{
		public Document(Stream stream, string name = "Новый документ")
			: base(stream)
		{
			IsOpened = false;
			Name = name;
		}

		public string Name { get; set; }

		public bool IsOpened { get; set; }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			IsOpened = false;
		}
	}
}