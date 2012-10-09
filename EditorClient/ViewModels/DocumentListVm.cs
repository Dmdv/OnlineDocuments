using System;
using System.Collections.ObjectModel;
using System.Timers;
using EditorClient.Commands;
using EditorClient.ServiceReference;
using OnlineEditor.Service;

namespace EditorClient.ViewModels
{
	/// <summary>
	/// View Model for documents.
	/// </summary>
	public class DocumentListVm : BaseViewModel
	{
		private const int Interval = 1000;
		private readonly Timer _timer;
		private ObservableCollection<Document> _documents;
		private DocumentVm _currentDocument;
		private Document _selectedDocument;

		public DocumentListVm()
		{
			Documents = new ObservableCollection<Document>();
			CreateCommand = new DelegateCommand<string>(Create, CanCreate);
			LoadCommand = new DelegateCommand<string>(Load, CanLoad);

			if (IsInDesignMode)
			{
				Documents.Add(new Document {Name = "doc1"});
				Documents.Add(new Document {Name = "doc2"});
			}
			else
			{
				_timer = new Timer { Enabled = true, Interval = Interval };
				_timer.Elapsed += OnTimer;
				_timer.Start();
			}
		}

		public DelegateCommand<string> CreateCommand { get; set; }

		public DelegateCommand<string> LoadCommand { get; set; }

		public Document SelectedDocument
		{
			get { return _selectedDocument; }
			set
			{
				_selectedDocument = value;
				LoadCommand.RaiseCanExecuteChanged();
			}
		}

		public DocumentVm CurrentDocument
		{
			get { return _currentDocument; }
			set
			{
				_currentDocument = value;
				OnPropertyChanged("CurrentDocument");
			}
		}

		public ObservableCollection<Document> Documents
		{
			get { return _documents; }
			set
			{
				_documents = value;
				OnPropertyChanged("Documents");
			}
		}

		private void Load(string name)
		{
			CurrentDocument = new DocumentVm(SelectedDocument.Name);
		}

		private bool CanLoad(string arg)
		{
			return Service.Online && SelectedDocument != null;
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
			return Service.Online;
		}

		void OnTimer(object sender, ElapsedEventArgs e)
		{
			Dispatcher.BeginInvoke(new Action(UpdateList));
		}

		private void UpdateList()
		{
			if (!Service.Online) return;
			var availableDocuments = Service.Proxy.AvailableDocuments();
			Documents = new ObservableCollection<Document>(availableDocuments);
		}
	}
}
