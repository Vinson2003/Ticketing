using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class MtRolepermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual MtPermission Permission { get; set; } = null!;

    public virtual MtRole Role { get; set; } = null!;
}
