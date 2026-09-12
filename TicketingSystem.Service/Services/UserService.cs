using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Services;

public class UserService(AppDbContext context) : IUserService
{
    private readonly AppDbContext _context = context;

    public List<DropdownItem> GetRoles()
    {
        return [.. _context.MtRoles.AsNoTracking()
            .OrderBy(i => i.Name)
            .Select(i => new DropdownItem
            {
                Id = i.Id,
                Name = i.Name
            })];
    }

    public BaseResponse<List<UserListResponse>> GetUsers(BasePaging paging, UserFilter filter)
    {
        var response = new BaseResponse<List<UserListResponse>>();

        var query = _context.MtUsers.AsNoTracking().AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(i =>
                i.Username.Contains(filter.Search) ||
                i.Name.Contains(filter.Search) ||
                (i.Email != null && i.Email.Contains(filter.Search))
            );
        }

        // Filter Role
        if (filter.RoleId.HasValue)
        {
            query = query.Where(i =>
                i.RoleId == filter.RoleId.Value
            );
        }

        // Filter Active
        if (filter.IsActive.HasValue)
        {
            query = query.Where(i =>
                i.IsActive == filter.IsActive.Value
            );
        }

        var totalFiltered = query.Count();

        var entity = query.Select(i => new UserListResponse
        {
            Id = i.Id,
            Username = i.Username,
            Name = i.Name,
            Email = i.Email,

            RoleId = i.RoleId,
            RoleName = i.Role.Name,

            IsActive = i.IsActive,

            CreatedAt = i.CreatedAt
        });

        var columnMappings = new Dictionary<string, Expression<Func<UserListResponse, object?>>>
        {
            ["username"] = i => i.Username,
            ["name"] = i => i.Name,
            ["email"] = i => i.Email,
            ["rolename"] = i => i.RoleName,
            ["isactive"] = i => i.IsActive,
            ["createdattext"] = i => i.CreatedAt
        };

        if (!string.IsNullOrEmpty(paging.Column) && columnMappings.TryGetValue( paging.Column, out var keySelector))
        {
            entity = paging.SortBy == Const.PAGING_SORT_ASC ? entity.OrderBy(keySelector) : entity.OrderByDescending(keySelector);
        }
        else
        {
            entity = entity.OrderBy(i => i.Name);
        }

        var data = entity.Skip(paging.Start).Take(paging.Length).ToList();

        var jakartaTimeZone =TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        data.ForEach(i =>
        {
            var localCreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(
                    i.CreatedAt,
                    DateTimeKind.Utc
                ),
                jakartaTimeZone
            );

            i.CreatedAtText = localCreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
        });

        response.Result = data;
        response.TotalFiltered = totalFiltered;

        response.Total = _context.MtUsers.AsNoTracking().Count();

        return response;
    }

    public BaseResponse<bool> CreateUser(CreateUserRequest request)
    {
        var response = new BaseResponse<bool>();

        var username = request.Username.Trim();

        var usernameExist = _context.MtUsers.AsNoTracking()
            .Any(i => i.Username == username);
        if (usernameExist)
        {
            response.Result = false;
            response.Message = "Username already exists.";
            return response;
        }

        var roleExist = _context.MtRoles.AsNoTracking()
            .Any(i => i.Id == request.RoleId);
        if (!roleExist)
        {
            response.Result = false;
            response.Message = "Invalid role.";
            return response;
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExist = _context.MtUsers.AsNoTracking()
                .Any(i => i.Email == request.Email);
            if (emailExist)
            {
                response.Result = false;
                response.Message = "Email already exists.";
                return response;
            }
        }

        var entity = new MtUser
        {
            Username = username,
            Password = PasswordHelper.HashPassword(request.Password),
            Name = request.Name.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            RoleId = request.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.MtUsers.Add(entity);
        _context.SaveChanges();

        response.Result = true;
        response.Message = "User created successfully.";

        return response;
    }

    public BaseResponse<UserDetailResponse> GetDetails(int id)
    {
        var response = new BaseResponse<UserDetailResponse>();

        var data = _context.MtUsers.AsNoTracking().Where(i => i.Id == id)
            .Select(i => new UserDetailResponse
            {
                Id = i.Id,
                Username = i.Username,
                Name = i.Name,
                Email = i.Email,

                RoleId = i.RoleId,
                RoleName = i.Role.Name,

                IsActive = i.IsActive,
                CreatedAt = i.CreatedAt
            })
            .FirstOrDefault();

        if (data == null)
        {
            response.Message = "User not found.";
            return response;
        }

        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        var localCreatedAt = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(
                data.CreatedAt,
                DateTimeKind.Utc
            ),
            jakartaTimeZone
        );

        data.CreatedAtText = localCreatedAt.ToString("dd/MM/yyyy HH:mm:ss");

        response.Result = data;
        response.Message = "Success";

        return response;
    }

    public BaseResponse<bool> UpdateUser(UpdateUserRequest request, int currentUserId)
    {
        var response = new BaseResponse<bool>();

        var entity = _context.MtUsers.FirstOrDefault(i => i.Id == request.Id);
        if (entity == null)
        {
            response.Result = false;
            response.Message = "User not found.";
            return response;
        }

        if (entity.Id == currentUserId && entity.RoleId != request.RoleId)
        {
            response.Result = false;
            response.Message = "You cannot change your own role.";
            return response;
        }

        var roleExist = _context.MtRoles.AsNoTracking()
            .Any(i => i.Id == request.RoleId);
        if (!roleExist)
        {
            response.Result = false;
            response.Message = "Invalid role.";
            return response;
        }

        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        if (email != null)
        {
            var emailExist = _context.MtUsers.AsNoTracking()
            .Any(i =>
                i.Email == email &&
                i.Id != request.Id
            );
            if (emailExist)
            {
                response.Result = false;
                response.Message = "Email already exists.";
                return response;
            }
        }

        entity.Name = request.Name.Trim();
        entity.Email = email;
        entity.RoleId = request.RoleId;

        _context.SaveChanges();

        response.Result = true;
        response.Message = "User updated successfully.";

        return response;
    }

    public BaseResponse<bool> ToggleActive(int id, int currentUserId)
    {
        var response = new BaseResponse<bool>();

        var entity = _context.MtUsers.FirstOrDefault(i => i.Id == id);
        if (entity == null)
        {
            response.Result = false;
            response.Message = "User not found.";
            return response;
        }

        if (entity.Id == currentUserId && entity.IsActive)
        {
            response.Result = false;
            response.Message = "You cannot deactivate your own account.";
            return response;
        }

        entity.IsActive = !entity.IsActive;

        _context.SaveChanges();

        response.Result = true;

        response.Message = entity.IsActive ? "User activated successfully." : "User deactivated successfully.";

        return response;
    }
}