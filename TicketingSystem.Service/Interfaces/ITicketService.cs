using TicketingSystem.Data.Entities;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces;

public interface ITicketService
{
    BaseResponse<List<ResponseListTicket>> GetTickets(BasePaging paging, TicketFilter filter);
    BaseResponse<DetailTicket> GetDetails(int id);
    BaseResponse<bool> CreateTicket(CreateTicketRequest request, int createdBy);
    TicketDropdownResponse GetDropdowns();
    BaseResponse<bool> UpdateTicket(UpdateTicketRequest request, int updatedBy);
    BaseResponse<bool> DeleteTicket(int id, int deletedBy);
}