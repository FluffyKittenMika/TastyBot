using System;

namespace TastyBot.FutureHeadPats
{
	/// <summary>
	/// Represents a single transaction of Future HeadPats (FHP)
	/// </summary>
	public class FhpTransaction
	{
		/// <summary>
		/// The amount of FHP to be sent
		/// </summary>
		public ulong FhpAmount { get; set; }
		/// <summary>
		/// The user that is sending FHP
		/// </summary>
		public ulong FhpSender { get; set; }
		/// <summary>
		/// The user that is receiving FHP
		/// </summary>
		public ulong FhpReceiver { get; set; }
		/// <summary>
		/// The unique ID of a transaction
		/// </summary>
		public Guid TransactionID { get; }

		public FhpTransaction()
		{
			TransactionID = Guid.NewGuid();
		}
	}
}
