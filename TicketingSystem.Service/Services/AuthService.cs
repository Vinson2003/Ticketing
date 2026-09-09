using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Context;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Services;

public class AuthService(AppDbContext context) : IAuthService
{
    private readonly AppDbContext _context = context;

    public BaseResponse<LoginResponse> Login(LoginRequest request)
    {
        var response = new BaseResponse<LoginResponse>();

        var user = _context.MtUsers.Include(i => i.Role)
            .FirstOrDefault(i => i.Username == request.Username && i.IsActive);

        if (user == null)
        {
            response.Message = "Invalid username or password.";
            return response;
        }

        if (!PasswordHelper.IsHashedPassword(user.Password))
        {
            if (user.Password != request.Password)
            {
                response.Message = "Invalid username or password.";
                return response;
            }

            user.Password = PasswordHelper.HashPassword(request.Password);
            _context.SaveChanges();
        }
        else
        {
            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                response.Message = "Invalid username or password.";
                return response;
            }
        }

        response.Result = new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleCode = user.Role.Code,
            RoleName = user.Role.Name
        };

        response.Message = "Login successful.";

        return response;
    }

    public BaseResponse<ProfileResponse> GetProfile(int userId)
    {
        var response = new BaseResponse<ProfileResponse>();

        var profile = _context.MtUsers.AsNoTracking()
            .Where(i => i.Id == userId && i.IsActive)
            .Select(i => new ProfileResponse
            {
                Id = i.Id,
                Username = i.Username,
                Name = i.Name,
                Email = i.Email,
                RoleName = i.Role.Name
            })
            .FirstOrDefault();

        if (profile == null)
        {
            response.Message = "User not found.";
            return response;
        }

        response.Result = profile;
        response.Message = "Success";

        return response;
    }

    public BaseResponse<bool> ChangePassword(int userId, ChangePasswordRequest request)
    {
        var response = new BaseResponse<bool>();

        var user = _context.MtUsers.FirstOrDefault(i =>
                i.Id == userId && i.IsActive);

        if (user == null)
        {
            response.Result = false;
            response.Message = "User not found.";
            return response;
        }

        if (!PasswordHelper.VerifyPassword(request.CurrentPassword, user.Password))
        {
            response.Result = false;
            response.Message = "Current password is incorrect.";
            return response;
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            response.Result = false;
            response.Message = "New password and confirmation do not match.";
            return response;
        }

        if (request.CurrentPassword == request.NewPassword)
        {
            response.Result = false;
            response.Message = "New password must be different from current password.";
            return response;
        }

        user.Password = PasswordHelper.HashPassword(request.NewPassword);

        _context.SaveChanges();

        response.Result = true;
        response.Message = "Password changed successfully.";

        return response;
    }
}