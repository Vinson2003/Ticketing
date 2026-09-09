using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

public class TicketHistoryService(AppDbContext context) : ITicketHistoryService
{
    private readonly AppDbContext _context = context;

    public void AddHistory(int ticketId, string action, string description, int createdBy, DateTime createdAt)
    {
        _context.TrTicketHistories.Add(new TrTicketHistory
        {
            TicketId = ticketId,
            Action = action,
            Description = description,
            CreatedBy = createdBy,
            CreatedAt = createdAt
        });
    }

    public List<string> GetChanges(TicketChangeSnapshot oldData, TicketChangeSnapshot newData)
    {
        var changes = new List<string>();

        if (oldData.Title != newData.Title)
        {
            changes.Add(
                $"Title changed from \"{oldData.Title}\" to \"{newData.Title}\"."
            );
        }

        if (oldData.Description != newData.Description)
        {
            changes.Add("Description updated.");
        }

        if (oldData.CategoryName != newData.CategoryName)
        {
            changes.Add(
                $"Category changed from {oldData.CategoryName} to {newData.CategoryName}."
            );
        }

        if (oldData.PriorityName != newData.PriorityName)
        {
            changes.Add(
                $"Priority changed from {oldData.PriorityName} to {newData.PriorityName}."
            );
        }

        if (oldData.StatusName != newData.StatusName)
        {
            changes.Add(
                $"Status changed from {oldData.StatusName} to {newData.StatusName}."
            );
        }

        if (oldData.AssignedToName != newData.AssignedToName)
        {
            changes.Add(
                $"Assigned To changed from " +
                $"{oldData.AssignedToName ?? "Unassigned"} to " +
                $"{newData.AssignedToName ?? "Unassigned"}."
            );
        }

        return changes;
    }

    public List<TicketHistoryResponse> GetTicketHistory(int ticketId)
    {
        var data = _context.TrTicketHistories.AsNoTracking()
            .Where(i => i.TicketId == ticketId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new TicketHistoryResponse
            {
                Id = i.Id,
                Action = i.Action,
                Description = i.Description,
                CreatedByName = i.CreatedByNavigation.Name,
                CreatedAt = i.CreatedAt
            })
            .ToList();

        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        data.ForEach(i =>
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(
                    i.CreatedAt,
                    DateTimeKind.Utc
                ),
                jakartaTimeZone
            );

            i.CreatedAtText = localTime.ToString("dd/MM/yyyy HH:mm:ss");
        });

        return data;
    }
}