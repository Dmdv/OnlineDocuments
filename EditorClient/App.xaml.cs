using System.Windows;
using EditorClient.ServiceReference;

namespace EditorClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void AppStartup(object sender, StartupEventArgs e)
		{
			Service.Start();
		}
	}
}
