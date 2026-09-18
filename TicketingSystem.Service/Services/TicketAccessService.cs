using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Data.Entities;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;

namespace TicketingSystem.Service.Services
{
    public class TicketAccessService : ITicketAccessService
    {
        
        public IQueryable<TrTicket> ApplyScope(IQueryable<TrTicket> query, int userId, string? roleCode)
        {
            var role = roleCode?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(role))
            {
                return query.Where(i => false);
            }

            return role switch
            {
                // Admin can access all tickets
                Const.ROLE_ADMIN => query,

                // Developer and Support can access tickets assigned to them
                Const.ROLE_DEVELOPER or Const.ROLE_SUPPORT => query.Where(i => i.AssignedTo == userId),

                // User can access tickets they created
                Const.ROLE_USER => query.Where(i => i.CreatedBy == userId),

                // Default case: no access
                _ => query.Where(i => false)
            };
        }
    }
}
