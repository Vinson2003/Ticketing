using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class TrTicketComment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual TrTicket Ticket { get; set; } = null!;

    public virtual MtUser User { get; set; } = null!;
}
