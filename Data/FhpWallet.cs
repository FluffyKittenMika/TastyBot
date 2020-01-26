namespace TastyBot.Data
{
	/// <summary>
	/// Represents the Future HeadPat (FHP) Wallet of a single user
	/// </summary>
	public class FhpWallet
	{
		/// <summary>
		/// The current amount of FHP the user has
		/// </summary>
		public ulong FhpAmount { get; set; }
		/// <summary>
		/// The user this wallet belongs to
		/// </summary>
		public string User { get; set; }
	}
}
