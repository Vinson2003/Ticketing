using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Data.Entities;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface ITicketHistoryService
    {
        void AddHistory(int ticketId, string action, string description, int createdBy, DateTime createdAt);

        List<string> GetChanges(TicketChangeSnapshot oldData, TicketChangeSnapshot newData);

        List<TicketHistoryResponse> GetTicketHistory(int ticketId);
    }
}
