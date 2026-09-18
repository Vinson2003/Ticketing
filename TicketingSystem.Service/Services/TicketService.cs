using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketingSystem.Service.Services;

public class TicketService(AppDbContext context, ITicketHistoryService ticketHistoryService, ITicketAccessService ticketAccessService) : ITicketService
{
    private readonly AppDbContext _context = context;
    private readonly ITicketHistoryService _ticketHistoryService = ticketHistoryService;
    private readonly ITicketAccessService _ticketAccessService = ticketAccessService;

    // GET: Ticket/Dropdowns
    public TicketDropdownResponse GetDropdowns()
    {
        return new TicketDropdownResponse
        {
            Categories = [.. _context.MtTicketCategories.AsNoTracking().Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .Select(i => new DropdownItem
                {
                    Id = i.Id,
                    Name = i.Name
                })],

            Priorities = [.. _context.MtTicketPriorities
                .AsNoTracking()
                .OrderBy(i => i.SortOrder)
                .Select(i => new DropdownItem
                {
                    Id = i.Id,
                    Name = i.Name
                })],

            Statuses = [.. _context.MtTicketStatuses
                .AsNoTracking()
                .OrderBy(i => i.SortOrder)
                .Select(i => new DropdownItem
                {
                    Id = i.Id,
                    Name = i.Name
                })],

            Users = [.. _context.MtUsers
                .AsNoTracking()
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .Select(i => new DropdownItem
                {
                    Id = i.Id,
                    Name = i.Name
                })],
        };
    }

    public BaseResponse<List<ResponseListTicket>> GetTickets(BasePaging paging, TicketFilter filter, int userId, string roleCode)
    {
        var response = new BaseResponse<List<ResponseListTicket>>();

        var query = _context.TrTickets.AsNoTracking()
            .Where(i => !i.IsDeleted);

        query = _ticketAccessService.ApplyScope(query, userId, roleCode);

        var total = query.Count();

        // Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(i =>
                i.TicketNo.Contains(filter.Search) ||
                i.Title.Contains(filter.Search) ||
                (i.Description != null && i.Description.Contains(filter.Search))
            );
        }

        // Filters
        if (filter.CategoryId.HasValue) query = query.Where(i => i.CategoryId == filter.CategoryId);
        if (filter.PriorityId.HasValue) query = query.Where(i => i.PriorityId == filter.PriorityId);
        if (filter.StatusId.HasValue) query = query.Where(i => i.StatusId == filter.StatusId);
        if (filter.CreatedBy.HasValue) query = query.Where(i => i.CreatedBy == filter.CreatedBy);
        if (filter.AssignedTo.HasValue) query = query.Where(i => i.AssignedTo == filter.AssignedTo);
        if (filter.CreatedFrom.HasValue) query = query.Where(i => i.CreatedAt >= filter.CreatedFrom.Value.Date);
        if (filter.CreatedTo.HasValue)
        {
            var createdTo = filter.CreatedTo.Value.Date.AddDays(1);
            query = query.Where(i => i.CreatedAt < createdTo);
        }

        var totalFiltered = query.Count();

        var entity = query.Select(i => new ResponseListTicket
        {
            Id = i.Id,
            TicketNo = i.TicketNo,
            Title = i.Title,
            Description = i.Description,

            CategoryId = i.CategoryId,
            CategoryName = i.Category.Name,

            PriorityId = i.PriorityId,
            PriorityName = i.Priority.Name,

            StatusId = i.StatusId,
            StatusName = i.Status.Name,

            CreatedBy = i.CreatedBy,
            CreatedByName = i.CreatedByNavigation.Name,

            AssignedTo = i.AssignedTo,
            AssignedToName = i.AssignedToNavigation != null ? i.AssignedToNavigation.Name : null,

            CreatedAt = i.CreatedAt
        });

        var columnMappings = new Dictionary<string, Expression<Func<ResponseListTicket, object?>>>
        {
            ["ticketno"] = i => i.TicketNo,
            ["title"] = i => i.Title,
            ["categoryname"] = i => i.CategoryName,
            ["priorityname"] = i => i.PriorityName,
            ["statusname"] = i => i.StatusName,
            ["createdbyname"] = i => i.CreatedByName,
            ["assignedtoname"] = i => i.AssignedToName,
            ["createdattext"] = i => i.CreatedAt
        };

        if (!string.IsNullOrEmpty(paging.Column) && columnMappings.TryGetValue(paging.Column, out var keySelector))
        {
            entity = paging.SortBy == Const.PAGING_SORT_ASC ? entity.OrderBy(keySelector) : entity.OrderByDescending(keySelector);
        }
        else
        {
            entity = entity.OrderByDescending(i => i.CreatedAt);
        }

        var data = entity.Skip(paging.Start).Take(paging.Length).ToList();

        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        data.ForEach(i =>
        {
            var localCreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(i.CreatedAt, DateTimeKind.Utc),
                jakartaTimeZone
            );

            i.CreatedAtText = localCreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
        });

        response.Result = data;
        response.TotalFiltered = totalFiltered;
        response.Total = total;

        return response;
    }

    public BaseResponse<DetailTicket> GetDetails(int id, int userId, string roleCode)
    {
        var response = new BaseResponse<DetailTicket>();

        var query = _context.TrTickets.AsNoTracking()
            .Where(i => i.Id == id && !i.IsDeleted);

        query = _ticketAccessService.ApplyScope(query, userId, roleCode);

        var detail = query.Select(i => new DetailTicket
        {
            Id = i.Id,
            TicketNo = i.TicketNo,
            Title = i.Title,
            Description = i.Description,

            CategoryId = i.CategoryId,
            CategoryName = i.Category.Name,

            PriorityId = i.PriorityId,
            PriorityName = i.Priority.Name,

            StatusId = i.StatusId,
            StatusName = i.Status.Name,

            CreatedBy = i.CreatedBy,
            CreatedByName = i.CreatedByNavigation.Name,

            AssignedTo = i.AssignedTo,

            AssignedToName = i.AssignedToNavigation != null ? i.AssignedToNavigation.Name : null,

            CreatedAt = i.CreatedAt
        })
        .FirstOrDefault();

        if (detail == null)
        {
            response.Message = "Ticket not found or access denied.";
            return response;
        }

        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        var localCreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(detail.CreatedAt, DateTimeKind.Utc),
            jakartaTimeZone
        );

        detail.CreatedAtText = localCreatedAt.ToString("dd/MM/yyyy HH:mm:ss");

        response.Result = detail;
        response.Message = "Success";

        return response;
    }

    public BaseResponse<bool> CreateTicket(CreateTicketRequest request, int createdBy, string roleCode)
    {
        var response = new BaseResponse<bool>();

        // validasi category
        var categoryExist = _context.MtTicketCategories
            .AsNoTracking()
            .Any(i => i.Id == request.CategoryId && i.IsActive);

        if (!categoryExist)
        {
            response.Result = false;
            response.Message = "Invalid category.";
            return response;
        }

        // validasi priority
        var priorityExist = _context.MtTicketPriorities
            .AsNoTracking()
            .Any(i => i.Id == request.PriorityId);

        if (!priorityExist)
        {
            response.Result = false;
            response.Message = "Invalid priority.";
            return response;
        }

        int? assignedTo = request.AssignedTo;

        switch (roleCode.ToUpper())
        {
            case Const.ROLE_ADMIN:
            break;

            case Const.ROLE_DEVELOPER:
            case Const.ROLE_SUPPORT:

                if (assignedTo.HasValue &&
                    assignedTo.Value != createdBy)
                {
                    response.Result = false;
                    response.Message = "You can only assign the ticket to yourself.";
                    return response;
                }

            break;

            case Const.ROLE_USER:
                assignedTo = null;
            break;

            default:

                response.Result = false;
                response.Message = "You are not allowed to create tickets.";
            return response;
        }

        // kalau AssignedTo ada, pastikan user valid
        if (assignedTo.HasValue)
        {
            var userExist = _context.MtUsers.AsNoTracking()
                .Any(i => i.Id == assignedTo.Value && i.IsActive);

            if (!userExist)
            {
                response.Result = false;
                response.Message = "Invalid assigned user.";
                return response;
            }
        }

        var now = DateTime.UtcNow;

        var entity = new TrTicket
        {
            TicketNo = $"TCK-{now:yyyyMMddHHmmssfff}",

            Title = request.Title,
            Description = request.Description,

            CategoryId = request.CategoryId,
            PriorityId = request.PriorityId,

            StatusId = Const.TICKET_STATUS_OPEN,

            CreatedBy = createdBy,
            AssignedTo = assignedTo,

            CreatedAt = now
        };

        _context.TrTickets.Add(entity);
        _context.SaveChanges();

        _ticketHistoryService.AddHistory(entity.Id, "CREATE", "Ticket created.", createdBy, now);

        _context.SaveChanges();

        response.Result = true;
        response.Message = "Ticket created successfully.";

        return response;
    }

    public BaseResponse<bool> UpdateTicket(UpdateTicketRequest request, int updatedBy, string roleCode)
    {
        var response = new BaseResponse<bool>();

        var query = _context.TrTickets
            .Include(i => i.Category)
            .Include(i => i.Priority)
            .Include(i => i.Status)
            .Include(i => i.AssignedToNavigation)
            .Where(i => !i.IsDeleted);

        query = _ticketAccessService.ApplyScope(query, updatedBy, roleCode);

        var entity = query.FirstOrDefault(i => i.Id == request.Id);
        if (entity == null)
        {
            response.Result = false;
            response.Message = "Ticket not found or access denied.";
            return response;
        }

        // Category
        var category = _context.MtTicketCategories
            .AsNoTracking()
            .FirstOrDefault(i => i.Id == request.CategoryId && i.IsActive);
        if (category == null)
        {
            response.Result = false;
            response.Message = "Invalid category.";
            return response;
        }

        // Priority
        var priority = _context.MtTicketPriorities.AsNoTracking()
            .FirstOrDefault(i => i.Id == request.PriorityId);
        if (priority == null)
        {
            response.Result = false;
            response.Message = "Invalid priority.";
            return response;
        }

        // Status
        var status = _context.MtTicketStatuses.AsNoTracking()
            .FirstOrDefault(i => i.Id == request.StatusId);
        if (status == null)
        {
            response.Result = false;
            response.Message = "Invalid status.";
            return response;
        }

        // Assigned User
        MtUser? assignedUser = null;

        if (request.AssignedTo.HasValue)
        {
            assignedUser = _context.MtUsers.AsNoTracking()
                .FirstOrDefault(i => i.Id == request.AssignedTo.Value && i.IsActive);
            if (assignedUser == null)
            {
                response.Result = false;
                response.Message = "Invalid assigned user.";
                return response;
            }
        }

        // Data sebelum update
        var oldData = new TicketChangeSnapshot
        {
            Title = entity.Title,
            Description = entity.Description,
            CategoryName = entity.Category.Name,
            PriorityName = entity.Priority.Name,
            StatusName = entity.Status.Name,
            AssignedToName = entity.AssignedToNavigation?.Name
        };

        // Data sesudah update
        var newData = new TicketChangeSnapshot
        {
            Title = request.Title,
            Description = request.Description,
            CategoryName = category.Name,
            PriorityName = priority.Name,
            StatusName = status.Name,
            AssignedToName = assignedUser?.Name
        };

        // Bandingkan sebelum entity diubah
        var changes = _ticketHistoryService.GetChanges(oldData, newData);

        // Update entity
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.CategoryId = request.CategoryId;
        entity.PriorityId = request.PriorityId;
        entity.StatusId = request.StatusId;
        entity.AssignedTo = request.AssignedTo;
        entity.UpdatedAt = DateTime.UtcNow;

        if (request.StatusId == Const.TICKET_STATUS_RESOLVED)
        {
            entity.ResolvedAt ??= DateTime.UtcNow;
        }

        if (request.StatusId == Const.TICKET_STATUS_CLOSED)
        {
            entity.ClosedAt ??= DateTime.UtcNow;
        }

        // Add history
        foreach (var change in changes)
        {
            _ticketHistoryService.AddHistory(
                entity.Id,
                "UPDATE",
                change,
                updatedBy,
                DateTime.UtcNow
            );
        }

        _context.SaveChanges();

        response.Result = true;
        response.Message = "Ticket updated successfully.";

        return response;
    }

    public BaseResponse<bool> DeleteTicket(int id, int deletedBy,string roleCode)
    {
        var response = new BaseResponse<bool>();

        var query = _context.TrTickets.Where(i => !i.IsDeleted);

        query = _ticketAccessService.ApplyScope(query, deletedBy, roleCode);

        var entity = query.FirstOrDefault(
            i => i.Id == id
        );

        if (entity == null)
        {
            response.Result = false;
            response.Message = "Ticket not found or access denied.";

            return response;
        }

        var now = DateTime.UtcNow;

        entity.IsDeleted = true;
        entity.UpdatedAt = now;

        _ticketHistoryService.AddHistory(
            entity.Id,
            "DELETE",
            "Ticket deleted.",
            deletedBy,
            now
        );

        _context.SaveChanges();

        response.Result = true;
        response.Message = "Ticket deleted successfully.";

        return response;
    }
}