using System;
using System.Runtime.Serialization;

namespace OnlineEditor.Service
{
	[DataContract]
	public class Document
	{
		[DataMember]
		public string Owner { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime CreationDate { get; set; }

		[DataMember]
		public bool ReadOnly { get; set; }

		public string UserName
		{
			get { return ReadOnly ? string.Format("{0} Read only, opened: {1}", Name, Owner) : Name; }
		}
	}
}