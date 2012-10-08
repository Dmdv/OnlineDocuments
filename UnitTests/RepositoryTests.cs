using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineEditor;
using OnlineEditor.Service;

namespace UnitTests
{
	[TestClass]
	public class RepositoryTests
	{
		[TestMethod]
		public void TryOpenNotExisting()
		{
			var repo = new DocumentsRepository();
			var result = repo.Open("dsds", "user1");
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Deleted);
		}

		[TestMethod]
		public void AssumeWillOpenOpenBySameUser()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");

			var result = repo.Open("doc1", "user1");
			var result2 = repo.Open("doc1", "user1");

			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);
			Assert.IsTrue(result2.Success);
			Assert.IsTrue(result2.State == State.Opened);
		}

		[TestMethod]
		public void AssumeWontOpenOpenByAnotherUser()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");

			var result = repo.Open("doc1", "user1");
			var result2 = repo.Open("doc1", "user2");

			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);
			Assert.IsTrue(!result2.Success);
			Assert.IsTrue(result2.State == State.Opened);
		}

		[TestMethod]
		public void TryWriteOpened()
		{
			const string Text = "sdasdasdas";

			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");

			var result = repo.Open("doc1", "user1");
			var result2 = repo.Write("doc1", "user1", Text);
			var result3 = repo.Open("doc1", "user1");

			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);
			Assert.IsTrue(result2.Success);
			Assert.IsTrue(result2.State == State.Opened);
			Assert.IsTrue(result3.Success);
			Assert.IsTrue(result3.State == State.Opened);
			Assert.IsTrue(result3.Buffer == Text);
		}

		[TestMethod]
		public void AssumeWriteInCreated()
		{
			const string Text = "sdasdasdas";

			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");

			var result = repo.Write("doc1", "user1", Text);

			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);
		}

		[TestMethod]
		public void AssumeWonWriteInOpendByAnotheruser()
		{
			const string Text = "sdasdasdas";

			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");

			var result = repo.Open("doc1", "user1");
			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.State == State.Opened);

			result = repo.Write("doc1", "user2", Text);
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.State == State.Opened);
		}

		[TestMethod]
		public void AssumeDocumentListCorrect()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");
			repo.Create("doc2", "user1");
			repo.Create("doc3", "user1");
			repo.Create("doc4", "user1");

			var docs = repo.AvailableDocuments();
			Assert.IsTrue(docs.Length == 4);
			Assert.IsTrue(docs[0] == "doc1");
			Assert.IsTrue(docs[1] == "doc2");
			Assert.IsTrue(docs[2] == "doc3");
			Assert.IsTrue(docs[3] == "doc4");
		}

		[TestMethod]
		public void AssumeOpenedDocumentsCorrect()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");
			repo.Create("doc2", "user1");
			repo.Create("doc3", "user1");
			repo.Create("doc4", "user1");

			repo.Open("doc2", "user2");

			var docs = repo.AvailableDocuments();
			Assert.IsTrue(docs.Length == 4);
			Assert.IsTrue(docs[0] == "doc1");
			Assert.IsTrue(docs[1] == "doc2 (read only: user2)");
			Assert.IsTrue(docs[2] == "doc3");
			Assert.IsTrue(docs[3] == "doc4");
		}

		[TestMethod]
		public void AssumeOpenedAndClosedDocumentsCorrect()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");
			repo.Create("doc2", "user1");
			repo.Create("doc3", "user1");
			repo.Create("doc4", "user1");

			repo.Open("doc2", "user2");
			repo.Close("doc2", "user2");

			var docs = repo.AvailableDocuments();
			Assert.IsTrue(docs.Length == 4);
			Assert.IsTrue(docs[0] == "doc1");
			Assert.IsTrue(docs[1] == "doc2");
			Assert.IsTrue(docs[2] == "doc3");
			Assert.IsTrue(docs[3] == "doc4");
		}

		[TestMethod]
		public void AssumeDeletedDocumentsListCorrect()
		{
			var repo = new DocumentsRepository();
			repo.Create("doc1", "user1");
			repo.Create("doc2", "user1");
			repo.Create("doc3", "user1");
			repo.Create("doc4", "user1");

			repo.Delete("doc2", "user2");

			var docs = repo.AvailableDocuments();
			Assert.IsTrue(docs.Length == 3);
			Assert.IsTrue(docs[0] == "doc1");
			Assert.IsTrue(docs[1] == "doc3");
			Assert.IsTrue(docs[2] == "doc4");
		}
	}
}
