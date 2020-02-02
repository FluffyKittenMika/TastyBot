namespace TastyBot.Data
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
		public string FhpSender { get; set; }
		/// <summary>
		/// The user that is receiving FHP
		/// </summary>
		public string FhpReceiver { get; set; }
	}
}
