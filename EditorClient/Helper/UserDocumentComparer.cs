using System.Collections.Generic;
using EditorClient.ViewModels;

namespace EditorClient.Helper
{
	internal class DocumentStatefulComparer : IEqualityComparer<DocumentVm>
	{
		public bool Equals(DocumentVm x, DocumentVm y)
		{
			return x.Name == y.Name && x.Document.ReadOnly == y.Document.ReadOnly;
		}

		public int GetHashCode(DocumentVm obj)
		{
			return 0;
		}
	}
}