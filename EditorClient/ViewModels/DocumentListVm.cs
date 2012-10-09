using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Timers;
using EditorClient.Commands;
using EditorClient.Helper;
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
		private ObservableCollection<DocumentVm> _documents;
		private DocumentVm _selectedDocument;

		private int _selectedIndex;

		public DocumentListVm()
		{
			Documents = new ObservableCollection<DocumentVm>();
			CreateCommand = new DelegateCommand<DocumentVm>(Create, CanCreate);
			LoadCommand = new DelegateCommand<DocumentVm>(Load, CanLoad);

			if (IsInDesignMode)
			{
				Documents.Add(new DocumentVm(new Document {Name = "doc1"}));
				Documents.Add(new DocumentVm(new Document {Name = "doc2"}));
			}
			else
			{
				_timer = new Timer { Enabled = true, Interval = Interval };
				_timer.Elapsed += OnTimer;
				_timer.Start();
			}
		}

		public DelegateCommand<DocumentVm> CreateCommand { get; set; }

		public DelegateCommand<DocumentVm> LoadCommand { get; set; }

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				_selectedIndex = value;
				OnPropertyChanged("SelectedIndex");
			}
		}

		public DocumentVm SelectedDocument
		{
			get { return _selectedDocument; }
			set
			{
				_selectedDocument = value;
				OnPropertyChanged("SelectedDocument");
				LoadCommand.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<DocumentVm> Documents
		{
			get { return _documents; }
			set
			{
				_documents = value;
				OnPropertyChanged("Documents");
			}
		}

		private void Load(DocumentVm doc)
		{
			SelectedDocument.Load();
		}

		private bool CanLoad(DocumentVm doc)
		{
			return Service.Online && SelectedDocument != null;
		}

		private void Create(DocumentVm doc)
		{
			var result = Service.Proxy.Create(null, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to create document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
		}

		private bool CanCreate(DocumentVm doc)
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
			var documents = Service.Proxy.AvailableDocuments().Select(x => new DocumentVm(x)).ToList();
			if (Documents.SequenceEqual(documents, new DocumentStatefulComparer())) return;

			if (SelectedDocument != null)
			{
				_selectedIndex = documents.Contains(SelectedDocument, new DocumentComparer())
				                 	? Documents.IndexOf(SelectedDocument)
				                 	: 0;
			}

			Documents = new ObservableCollection<DocumentVm>(documents);
			OnPropertyChanged("SelectedIndex");
		}
	}
}
