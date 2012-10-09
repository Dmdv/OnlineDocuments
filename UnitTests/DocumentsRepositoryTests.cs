using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineEditor;
using OnlineEditor.Data;
using OnlineEditor.Managers;
using OnlineEditor.Service;

namespace UnitTests
{
	[TestClass]
	public class DocumentsRepositoryTests
	{
		[TestMethod]
		public void TestDeleteNotExisting()
		{
			var manager = new DocumentsRepository();
			var result = manager.Delete("sda", "dsa");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Deleted);
		}

		[TestMethod]
		public void AssumeDocCreated()
		{
			var manager = new DocumentsRepository();
			var document = manager.Create("doc1", "user1");
			Assert.IsTrue(document.State == State.Created);
			Assert.IsTrue(document.Success);
		}

		[TestMethod]
		public void AssumeTwoSameDocsCreated()
		{
			var mng = new MemoryDocuments();
			var container = new DocumentsContainer(mng);

			var result = container.Create("doc1", "user1");
			Assert.IsTrue(result.State == State.Created);
			Assert.IsTrue(result.Success);

			var result1 = container.Create("doc1", "user1");
			Assert.IsTrue(result1.State == State.Created);
			Assert.IsTrue(result1.Success);
		}

		[TestMethod]
		public void AssumeOpenedWontDelete()
		{
			// create
			var manager = new DocumentsRepository();
			var document = manager.Create("doc1", "user1");
			Assert.IsTrue(document.State == State.Created);
			Assert.IsTrue(document.Success);

			// open
			var result = manager.Open("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);

			// delete while opened.
			result = manager.Delete("doc1", "user2");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Opened);
		}

		[TestMethod]
		public void AssumeOpenedWillDelete()
		{
			// create
			var manager = new DocumentsRepository();
			var document = manager.Create("doc1", "user1");
			Assert.IsTrue(document.State == State.Created);
			Assert.IsTrue(document.Success);

			// open
			var result = manager.Open("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);

			// delete while opened.
			result = manager.Delete("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Deleted);
		}

		[TestMethod]
		public void AssumeOpenedWillCloseAndDelete()
		{
			// create
			var manager = new DocumentsRepository();
			var document = manager.Create("doc1", "user1");
			Assert.IsTrue(document.State == State.Created);
			Assert.IsTrue(document.Success);

			// open
			var result = manager.Open("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);

			// close while opened.
			result = manager.Close("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Closed);

			// deleted closed.
			result = manager.Delete("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State != State.Closed);
		}

		[TestMethod]
		public void AssumeOpenedAnotheruserWontClose()
		{
			// create
			var manager = new DocumentsRepository();
			var document = manager.Create("doc1", "user1");
			Assert.IsTrue(document.State == State.Created);
			Assert.IsTrue(document.Success);

			// open
			var result = manager.Open("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);

			// close while opened.
			result = manager.Close("doc1", "user2");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Opened);
		}

		[TestMethod]
		public void AssertDeletedWontOpen()
		{
			var manager = new DocumentsRepository();
			manager.Create("doc1", "user1");
			var result = manager.Delete("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Deleted);

			result = manager.Open("doc1", "user1");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Deleted);
		}

		[TestMethod]
		public void AssertReadSameText()
		{
			var manager = new DocumentsRepository();
			manager.Create("doc1", "user1");
			var result = manager.Delete("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Deleted);

			result = manager.Open("doc1", "user1");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Deleted);
		}
	}
}
