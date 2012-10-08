using System.Windows;

namespace EditorClient.ServiceReference
{
	public class DefaultLogger : ILogger
	{
		public void LogError(string message)
		{
			MessageBox.Show(message, "Exception...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}