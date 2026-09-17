using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class MtUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual MtRole Role { get; set; } = null!;

    public virtual ICollection<TrTicket> TrTicketAssignedToNavigations { get; set; } = new List<TrTicket>();

    public virtual ICollection<TrTicketComment> TrTicketComments { get; set; } = new List<TrTicketComment>();

    public virtual ICollection<TrTicket> TrTicketCreatedByNavigations { get; set; } = new List<TrTicket>();

    public virtual ICollection<TrTicketHistory> TrTicketHistories { get; set; } = new List<TrTicketHistory>();
}
