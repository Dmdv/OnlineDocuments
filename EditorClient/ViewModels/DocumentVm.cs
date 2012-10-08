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
		private readonly bool _isOpened;

		public DocumentVm(string name)
		{
			Name = name;

			SaveCommand = new DelegateCommand<string>(Save, CanSave);
			CloseCommand = new DelegateCommand<string>(Close, CanClose);
			DeleteCommand = new DelegateCommand<string>(Delete, CanDelete);

			var result = Service.Proxy.Open(Name, Service.CurrentUser);
			if (result.Success)
			{
				Text = result.Buffer;
				_isOpened = true;
			}
			else switch (result.State)
			{
				case State.Opened:
					Logger.LogError(string.Format("{0} is opened for edit by another user", Name));
					break;
				case State.Deleted:
					Logger.LogError(string.Format("{0} was deleted by another user", Name));
					break;
			}
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

		private void Close(string obj)
		{
			var result = Service.Proxy.Close(Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to close document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
		}

		private bool CanClose(string arg)
		{
			return _isOpened;
		}

		private void Delete(string obj)
		{
			var result = Service.Proxy.Delete(Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to delete document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
		}

		private bool CanDelete(string arg)
		{
			return _isOpened;
		}

		private void Save(string text)
		{
			Service.Proxy.Write(Name, Service.CurrentUser, text);
		}

		private bool CanSave(string arg)
		{
			return Service.Online && _isOpened;
		}
	}
}