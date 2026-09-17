using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Services
{
    public class TicketCommentService(AppDbContext context, ITicketAccessService ticketAccessService) : ITicketCommentService
    {
        private readonly AppDbContext _context = context;

        private readonly ITicketAccessService _ticketAccessService = ticketAccessService;

        public List<TicketCommentResponse>? GetComments(int ticketId, int userId, string roleCode)
        {
            var ticketQuery = _context.TrTickets.AsNoTracking()
                .Where(i => i.Id == ticketId && !i.IsDeleted);

            ticketQuery = _ticketAccessService.ApplyScope(ticketQuery, userId, roleCode);

            if (!ticketQuery.Any())
            {
                return null;
            }

            var data = _context.TrTicketComments.AsNoTracking()
                .Where(i => i.TicketId == ticketId)
                .OrderBy(i => i.CreatedAt)
                .Select(i => new TicketCommentResponse
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    UserName = i.User.Name,
                    Comment = i.Comment,
                    CreatedAt = i.CreatedAt
                })
                .ToList();

            var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time" );

            data.ForEach(i =>
            {
                var localTime =
                    TimeZoneInfo.ConvertTimeFromUtc(
                        DateTime.SpecifyKind(
                            i.CreatedAt,
                            DateTimeKind.Utc
                        ),
                        jakartaTimeZone
                    );

                i.CreatedAtText = localTime.ToString("dd MMM yyyy · HH:mm");
            });

            return data;
        }

        public BaseResponse<bool> AddComment(AddTicketCommentRequest request, int userId, string roleCode)
        {
            var response = new BaseResponse<bool>();

            var ticketQuery = _context.TrTickets
                .Where(i => i.Id == request.TicketId && !i.IsDeleted);

            ticketQuery = _ticketAccessService.ApplyScope(ticketQuery, userId, roleCode);

            var ticket = ticketQuery.FirstOrDefault();
            if (ticket == null)
            {
                response.Result = false;
                response.Message = "Ticket not found or access denied.";

                return response;
            }

            var comment = request.Comment.Trim();

            if (string.IsNullOrWhiteSpace(comment))
            {
                response.Result = false;
                response.Message = "Comment is required.";
                return response;
            }

            _context.TrTicketComments.Add(
                new TrTicketComment
                {
                    TicketId = request.TicketId,
                    UserId = userId,
                    Comment = comment,
                    CreatedAt = DateTime.UtcNow
                }
            );

            _context.SaveChanges();

            response.Result = true;
            response.Message = "Comment added successfully.";

            return response;
        }
    }
}
