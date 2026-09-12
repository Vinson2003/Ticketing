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
        public IQueryable<TrTicket> ApplyScope(IQueryable<TrTicket> query, int userId, string roleCode)
        {
            roleCode = roleCode.ToUpper();

            if (roleCode == Const.ROLE_ADMIN)
            {
                return query;
            }

            if (roleCode == Const.ROLE_DEVELOPER || roleCode == Const.ROLE_SUPPORT)
            {
                return query.Where(i =>
                    i.AssignedTo == userId
                );
            }

            if (roleCode == Const.ROLE_USER)
            {
                return query.Where(i =>
                    i.CreatedBy == userId
                );
            }

            return query.Where(i => false);
        }
    }
}
