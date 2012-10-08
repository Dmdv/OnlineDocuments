using System.Runtime.Serialization;

namespace OnlineEditor.Service
{
	[DataContract]
	public enum State
	{
		[EnumMember]
		Created,
		[EnumMember]
		Deleted,
		[EnumMember]
		Opened,
		[EnumMember]
		Closed
	}
}