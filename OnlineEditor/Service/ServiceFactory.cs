using System.ServiceModel;
using System.ServiceModel.Channels;

namespace OnlineEditor.Service
{
	public static class ServiceFactory
	{
		public static Binding Binding
		{
			get { return new WSHttpBinding { Name = Name }; }
		}

		public static string Uri
		{
			get { return "http://localhost:8739/Design_Time_Addresses/WcfServiceLibrary1/Service1/"; }
		}

		public static string Name
		{
			get { return "OnlineEditor.Service.DocumentRepository"; }
		}
	}
}