using System;
using EditorClient.Commands;
using EditorClient.ServiceReference;
using OnlineEditor.Service;

namespace EditorClient.ViewModels
{
	/// <summary>
	/// View Model for document.
	/// </summary>
	public class DocumentVm : BaseViewModel
	{
		public DocumentVm(Document doc)
		{
			Document = doc;
			SaveCommand = new DelegateCommand<DocumentVm>(Save, CanSave);
			CloseCommand = new DelegateCommand<DocumentVm>(Close, CanClose);
			DeleteCommand = new DelegateCommand<DocumentVm>(Delete, CanDelete);
		}

		public DelegateCommand<DocumentVm> SaveCommand { get; set; }
		public DelegateCommand<DocumentVm> CloseCommand { get; set; }
		public DelegateCommand<DocumentVm> DeleteCommand { get; set; }

		public string Text
		{
			get { return Document.Text; }
			set
			{
				Document.Text = value;
				OnPropertyChanged("Text");
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		public string Name
		{
			get { return Document.Name; }
			set
			{
				Document.Name = value;
				OnPropertyChanged("Name");
			}
		}

		public Document Document { get; private set; }

		public void Load()
		{
			Dispatcher.BeginInvoke(new Action(LoadDocument));
		}

		private void LoadDocument()
		{
			var result = Service.Proxy.Open(Name, Service.CurrentUser);
			if (result.Success)
			{
				Text = result.Buffer;
			}
			else
				switch (result.State)
				{
					case State.Opened:
						Logger.LogError(string.Format("{0} is opened for edit by another user", Name));
						break;
					case State.Deleted:
						Logger.LogError(string.Format("{0} was deleted by another user", Name));
						break;
					default:
						Text = "Failed to load text, Error: " + result.Buffer;
						break;
				}
		}

		private void CloseDocument()
		{
			var result = Service.Proxy.Close(Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to close document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
			Text = string.Empty;
		}

		private void DeleteDocument()
		{
			var result = Service.Proxy.Delete(Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to delete document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
			Text = string.Empty;
		}

		private void Close(DocumentVm obj)
		{
			Dispatcher.BeginInvoke(new Action(CloseDocument));
		}

		private bool CanClose(DocumentVm arg)
		{
			return Service.Online;
		}

		private void Delete(DocumentVm obj)
		{
			Dispatcher.BeginInvoke(new Action(DeleteDocument));
		}

		private bool CanDelete(DocumentVm arg)
		{
			return Service.Online;
		}

		private void Save(DocumentVm text)
		{
			var result = Service.Proxy.Write(Name, Service.CurrentUser, Text);
			if (!result.Success)
			{
				Logger.LogError(result.Buffer);
			}
		}

		private bool CanSave(DocumentVm arg)
		{
			return Service.Online;
		}
	}
}