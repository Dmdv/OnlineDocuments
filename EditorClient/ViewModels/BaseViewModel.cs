using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using EditorClient.ServiceReference;

namespace EditorClient.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected BaseViewModel()
		{
			Logger = new DefaultLogger();
		}

		protected Dispatcher Dispatcher
		{
			get { return Application.Current.Dispatcher; }
		}

		protected ILogger Logger { get; set; }

		internal static bool IsInDesignMode(DependencyObject dependency)
		{
			return DesignerProperties.GetIsInDesignMode(dependency);
		}

		protected void OnPropertyChanged(string property)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(property));
		}

		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, e);
		}
	}
}