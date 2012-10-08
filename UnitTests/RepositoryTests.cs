using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineEditor;

namespace UnitTests
{
	[TestClass]
	public class RepositoryTests
	{
		[TestMethod]
		public void TryOpenNotExisting()
		{
			var repo = new DocumentsRepository();
			Assert.IsNull(repo.Open("dsds"));
		}

		[TestMethod]
		public void TryOpenOpened()
		{
			var repo = new DocumentsRepository();
			var document = repo.Create();
			Assert.IsNotNull(repo.Open(document.Name));
			Assert.IsNull(repo.Open(document.Name));
		}
	}
}
