using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface IAuthService
    {
        BaseResponse<LoginResponse> Login(LoginRequest request);

        BaseResponse<ProfileResponse> GetProfile(int userId);

        BaseResponse<bool> ChangePassword(int userId, ChangePasswordRequest request);
    }
}
