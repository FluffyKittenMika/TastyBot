using System;
using System.Collections.Generic;

namespace FutureHeadPats.Entities
{
	public class FhpBooking
	{
		public Dictionary<ulong, FhpUser> Users { get; set; }
		public Dictionary<Guid, FhpTransaction> TransactionLog { get; set; }
	}
}
