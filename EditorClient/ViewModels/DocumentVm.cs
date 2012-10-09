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
		private string _name;
		private string _text;
		private bool _isOpened;

		public DocumentVm(string name)
		{
			Name = name;

			SaveCommand = new DelegateCommand<string>(Save, CanSave);
			CloseCommand = new DelegateCommand<string>(Close, CanClose);
			DeleteCommand = new DelegateCommand<string>(Delete, CanDelete);

			Dispatcher.BeginInvoke(new Action(LoadDocument));
		}

		public DelegateCommand<string> SaveCommand { get; set; }
		public DelegateCommand<string> CloseCommand { get; set; }
		public DelegateCommand<string> DeleteCommand { get; set; }

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				OnPropertyChanged("Text");
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		private void LoadDocument()
		{
			var result = Service.Proxy.Open(Name, Service.CurrentUser);
			if (result.Success)
			{
				_isOpened = true;
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

		private void Close(string obj)
		{
			Dispatcher.BeginInvoke(new Action(CloseDocument));
		}

		private bool CanClose(string arg)
		{
			return Service.Online && _isOpened;
		}

		private void Delete(string obj)
		{
			Dispatcher.BeginInvoke(new Action(DeleteDocument));
		}

		private bool CanDelete(string arg)
		{
			return Service.Online && _isOpened;
		}

		private void Save(string text)
		{
			var result = Service.Proxy.Write(Name, Service.CurrentUser, Text);
			if (!result.Success)
			{
				Logger.LogError(result.Buffer);
			}
		}

		private bool CanSave(string arg)
		{
			return Service.Online && _isOpened;
		}
	}
}