using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class TrTicket
{
    public int Id { get; set; }

    public string TicketNo { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int PriorityId { get; set; }

    public int StatusId { get; set; }

    public int CreatedBy { get; set; }

    public int? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual MtUser? AssignedToNavigation { get; set; }

    public virtual MtTicketCategory Category { get; set; } = null!;

    public virtual MtUser CreatedByNavigation { get; set; } = null!;

    public virtual MtTicketPriority Priority { get; set; } = null!;

    public virtual MtTicketStatus Status { get; set; } = null!;

    public virtual ICollection<TrTicketHistory> TrTicketHistories { get; set; } = new List<TrTicketHistory>();
}
