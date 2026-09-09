using System;
using System.Collections.Generic;

namespace TicketingSystem.Data.Entities;

public partial class MtPermission
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Display { get; set; } = null!;

    public int Seq { get; set; }

    public int SubSeq { get; set; }

    public virtual ICollection<MtRolepermission> MtRolepermissions { get; set; } = new List<MtRolepermission>();
}
