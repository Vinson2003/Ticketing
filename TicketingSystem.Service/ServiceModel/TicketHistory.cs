using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class TicketChangeSnapshot
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string CategoryName { get; set; } = null!;
        public string PriorityName { get; set; } = null!;
        public string StatusName { get; set; } = null!;

        public string? AssignedToName { get; set; }
    }

    public class TicketHistoryResponse
    {
        public int Id { get; set; }

        public string Action { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CreatedByName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string CreatedAtText { get; set; } = null!;
    }
}
