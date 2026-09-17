using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface ITicketCommentService
    {
        List<TicketCommentResponse>? GetComments(int ticketId, int userId, string roleCode);

        BaseResponse<bool> AddComment(AddTicketCommentRequest request, int userId, string roleCode);
    }
}
