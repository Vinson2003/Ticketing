using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class DashboardResponse
    {
        public int TotalTickets { get; set; }

        public int OpenTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ResolvedTickets { get; set; }

        public int ClosedTickets { get; set; }

        public List<DashboardChartItem> TicketsByStatus { get; set; } = [];

        public List<DashboardChartItem> TicketsByPriority { get; set; } = [];

        public List<DashboardRecentTicket> RecentTickets { get; set; } = [];
    }

    public class DashboardChartItem
    {
        public string Label { get; set; } = null!;

        public int Total { get; set; }
    }

    public class DashboardRecentTicket
    {
        public int Id { get; set; }

        public string TicketNo { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string StatusName { get; set; } = null!;

        public string PriorityName { get; set; } = null!;

        public string CreatedByName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string CreatedAtText { get; set; } = null!;
    }
}
