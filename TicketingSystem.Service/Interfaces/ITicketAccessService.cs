using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Data.Entities;

namespace TicketingSystem.Service.Interfaces
{
    public interface ITicketAccessService
    {
        IQueryable<TrTicket> ApplyScope(IQueryable<TrTicket> query, int userId, string roleCode);
    }
}
