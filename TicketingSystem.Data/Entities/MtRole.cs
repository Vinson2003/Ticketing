using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class MtRole
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<MtRolepermission> MtRolepermissions { get; set; } = new List<MtRolepermission>();

    public virtual ICollection<MtUser> MtUsers { get; set; } = new List<MtUser>();
}
