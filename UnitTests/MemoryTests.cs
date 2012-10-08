using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineEditor.Managers;

namespace UnitTests
{
	[TestClass]
	public class MemoryTests
	{
		[TestMethod]
		public void ReadAndWriteDocument()
		{
			const string Msg = "dfsdhghdfghsdf;gsdfhg";
			var docs = new MemoryDocuments();
			var document = docs.Create("doc1");
			docs.Write(document, Msg);
			var text = docs.Read(document);
			Assert.IsTrue(text == Msg);
		}
	}
}