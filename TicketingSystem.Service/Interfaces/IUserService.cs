using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface IUserService
    {
        BaseResponse<List<UserListResponse>> GetUsers(BasePaging paging, UserFilter filter);

        BaseResponse<UserDetailResponse> GetDetails(int id);

        BaseResponse<bool> CreateUser(CreateUserRequest request);

        BaseResponse<bool> UpdateUser(UpdateUserRequest request);

        BaseResponse<bool> ToggleActive(int id, int currentUserId);

        List<DropdownItem> GetRoles();
    }
}
