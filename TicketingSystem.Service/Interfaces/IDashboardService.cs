using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface IDashboardService
    {
        DashboardResponse GetDashboard(int userId, string roleCode);
    }
}
