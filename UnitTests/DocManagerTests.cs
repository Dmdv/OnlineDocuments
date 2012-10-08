using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineEditor;
using OnlineEditor.Managers;

namespace UnitTests
{
	[TestClass]
	public class DocManagerTests
	{
		[TestMethod]
		public void AssumeNamesUnique()
		{
			var mng = new MemoryDocuments();
			var container = new DocumentsContainer(mng);
			var document1 = container.Create("doc1");
			var document2 = container.Create("doc1");
			Assert.IsTrue(document1.Name != document2.Name);
		}

		[TestMethod]
		public void AssumeDocNameCorrect()
		{
			var manager = new DocumentsRepository();
			var document = manager.Create();
			Assert.IsTrue(document.Name == "Новый документ");
		}

		[TestMethod]
		public void AssertDocumentDeleted()
		{
			var manager = new DocumentsRepository();
			var document = manager.Create();
			try
			{
				Assert.IsTrue(manager.Delete(document));
			}
			catch (NotSupportedException)
			{
			}
		}
	}
}
