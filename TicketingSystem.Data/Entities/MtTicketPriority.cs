using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class MtTicketPriority
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual ICollection<TrTicket> TrTickets { get; set; } = new List<TrTicket>();
}
