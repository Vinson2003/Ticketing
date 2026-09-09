using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class TrTicketHistory
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string Action { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual MtUser CreatedByNavigation { get; set; } = null!;

    public virtual TrTicket Ticket { get; set; } = null!;
}
