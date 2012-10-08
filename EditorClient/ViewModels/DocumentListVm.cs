using System.Collections.ObjectModel;
using System.Timers;
using EditorClient.Commands;
using EditorClient.ServiceReference;

namespace EditorClient.ViewModels
{
	/// <summary>
	/// View Model for documents.
	/// </summary>
	public class DocumentListVm : BaseViewModel
	{
		private const int Interval = 1000;
		private readonly Timer _timer;
		private ObservableCollection<string> _documents;

		public DocumentListVm()
		{
			CreateCommand = new DelegateCommand<string>(Create, CanCreate);
			Documents = new ObservableCollection<string>();
			_timer = new Timer {Enabled = true, Interval = Interval};
			_timer.Elapsed += OnTimer;
			_timer.Start();
		}

		private void Create(string name)
		{
			var result = Service.Proxy.Create(name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to create document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
		}

		private bool CanCreate(string arg)
		{
			return Service.Online && !string.IsNullOrEmpty(arg);
		}

		public DelegateCommand<string> CreateCommand { get; set; }

		public ObservableCollection<string> Documents
		{
			get { return _documents; }
			set
			{
				_documents = value;
				OnPropertyChanged("Documents");
			}
		}

		void OnTimer(object sender, ElapsedEventArgs e)
		{
			var availableDocuments = Service.Proxy.AvailableDocuments();
			Documents.Clear();
			Documents = new ObservableCollection<string>(availableDocuments);
		}
	}
}
