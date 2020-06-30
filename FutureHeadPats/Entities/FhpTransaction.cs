using System;

namespace FutureHeadPats.Entities
{
	/// <summary>
	/// Represents a single transaction of Future HeadPats (FHP)
	/// </summary>
	public class FhpTransaction
	{
		public ulong FhpAmount { get; set; }
		public ulong FhpSender { get; set; }
		public ulong FhpReceiver { get; set; }
		public Guid TransactionID { get; }

		public FhpTransaction()
		{
			TransactionID = Guid.NewGuid();
		}
	}
}
