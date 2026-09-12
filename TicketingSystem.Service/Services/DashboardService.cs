using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Context;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Services;

public class DashboardService(AppDbContext context, ITicketAccessService ticketAccessService) : IDashboardService
{
    private readonly AppDbContext _context = context;
    private readonly ITicketAccessService _ticketAccessService = ticketAccessService;

    public DashboardResponse GetDashboard(int userId, string roleCode)
    {
        var query = _context.TrTickets.AsNoTracking()
            .Where(i => !i.IsDeleted);

        query = _ticketAccessService.ApplyScope(query, userId, roleCode);

        var response = new DashboardResponse
        {
            TotalTickets = query.Count(),

            OpenTickets = query.Count(i => i.StatusId == Const.TICKET_STATUS_OPEN),

            InProgressTickets = query.Count(i => i.StatusId == Const.TICKET_STATUS_IN_PROGRESS),

            ResolvedTickets = query.Count(i => i.StatusId == Const.TICKET_STATUS_RESOLVED),

            ClosedTickets = query.Count(i => i.StatusId == Const.TICKET_STATUS_CLOSED),

            TicketsByStatus = [.. query.GroupBy(i => i.Status.Name)
                .Select(i => new DashboardChartItem
                {
                    Label = i.Key,
                    Total = i.Count()
                })
                .OrderByDescending(i => i.Total)],

            TicketsByPriority = [.. query
                .GroupBy(i => i.Priority.Name)
                .Select(i => new DashboardChartItem
                {
                    Label = i.Key,
                    Total = i.Count()
                })
                .OrderByDescending(i => i.Total)],

            RecentTickets = [.. query
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .Select(i => new DashboardRecentTicket
                {
                    Id = i.Id,
                    TicketNo = i.TicketNo,
                    Title = i.Title,
                    StatusName = i.Status.Name,
                    PriorityName = i.Priority.Name,
                    CreatedByName = i.CreatedByNavigation.Name,
                    CreatedAt = i.CreatedAt
                })]
        };

        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        response.RecentTickets.ForEach(i =>
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(i.CreatedAt, DateTimeKind.Utc),
                jakartaTimeZone
            );

            i.CreatedAtText = localTime.ToString("dd/MM/yyyy HH:mm:ss");
        });

        return response;
    }
}