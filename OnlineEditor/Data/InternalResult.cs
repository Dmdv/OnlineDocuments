using System;

namespace OnlineEditor.Data
{
	internal class InternalResult
	{
		public InternalResult()
		{
			Success = true;
		}

		public Exception Exception { get; set; }

		public bool Success { get; set; }

		public override string ToString()
		{
			return Exception == null ? string.Empty : Exception.ToString();
		}
	}
}