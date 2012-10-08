using System.Runtime.Serialization;

namespace OnlineEditor.Service
{
	[DataContract]
	public class Result
	{
		[DataMember]
		public State State { get; set; }

		[DataMember]
		public bool Success { get; set; }

		[DataMember]
		public string Buffer { get; set; }
	}
}
