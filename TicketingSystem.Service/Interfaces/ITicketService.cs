using TicketingSystem.Data.Entities;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces;

public interface ITicketService
{
    BaseResponse<List<ResponseListTicket>> GetTickets(BasePaging paging, TicketFilter filter, int userId, string roleCode);
    BaseResponse<DetailTicket> GetDetails(int id, int userId, string roleCode);
    BaseResponse<bool> CreateTicket(CreateTicketRequest request, int createdBy, string roleCode);
    TicketDropdownResponse GetDropdowns();
    BaseResponse<bool> UpdateTicket(UpdateTicketRequest request, int updatedBy, string roleCode);
    BaseResponse<bool> DeleteTicket(int id, int deletedBy, string roleCode);
}