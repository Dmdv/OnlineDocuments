using System.Collections.Generic;
using EditorClient.ViewModels;

namespace EditorClient.Helper
{
	internal class DocumentComparer : IEqualityComparer<DocumentVm>
	{
		public bool Equals(DocumentVm x, DocumentVm y)
		{
			return x.Name == y.Name;
		}

		public int GetHashCode(DocumentVm obj)
		{
			return 0;
		}
	}
}
