using System;
using System.Collections.Generic;

namespace TastyBot.FutureHeadPats
{
	public class FhpBooking
	{
		public Dictionary<ulong, FhpUser> Users { get; set; }
		public Dictionary<Guid, FhpTransaction> TransactionLog { get; set; }
	}
}
