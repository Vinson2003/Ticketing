using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Services;

public class PermissionService(AppDbContext context) : IPermissionService
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

    public List<RequestPermsRoleList> GetPermsRole(int roleId)
    {
        var query = _context.MtRolepermissions.AsNoTracking().AsQueryable();

        if (roleId > 0)
        {
            query = query.Where(i => i.RoleId == roleId);
        }

        return [.. query.Select(i => new RequestPermsRoleList
        {
            RoleId = i.RoleId,
            Role = i.Role.Name,

            PermissionId = i.PermissionId,
            Description = i.Permission.Description,
            Display = i.Permission.Display
        })];
    }

    public List<RolePermissionItem> GetRolePermissions(int roleId)
    {
        var selectedPermissions = _context.MtRolepermissions.AsNoTracking().Where(i => i.RoleId == roleId)
            .Select(i => i.PermissionId);

        return [.. _context.MtPermissions.AsNoTracking().OrderBy(i => i.Seq).ThenBy(i => i.SubSeq)
            .Select(i => new RolePermissionItem
            {
                PermissionId = i.Id,
                Description = i.Description,
                Display = i.Display,

                IsChecked = selectedPermissions.Contains(i.Id)
            })];
    }

    public BaseResponse<bool> SaveRolePermissions(SaveRolePermissionRequest request)
    {
        var response = new BaseResponse<bool>();

        var roleExist = _context.MtRoles.AsNoTracking().Any(i => i.Id == request.RoleId);
        if (!roleExist)
        {
            response.Result = false;
            response.Message = "Invalid role.";
            return response;
        }

        var permissionIds = request.PermissionIds.Distinct().ToList();

        var validPermissionIds = _context.MtPermissions.AsNoTracking()
            .Where(i => permissionIds.Contains(i.Id))
            .Select(i => i.Id)
            .ToList();

        if (validPermissionIds.Count != permissionIds.Count)
        {
            response.Result = false;
            response.Message = "Invalid permission.";
            return response;
        }

        var existingPermissions = _context.MtRolepermissions.Where(i => i.RoleId == request.RoleId)
            .ToList();

        _context.MtRolepermissions.RemoveRange(existingPermissions);

        var permissions = permissionIds.Select(permissionId => new MtRolepermission
        {
            RoleId = request.RoleId,
            PermissionId = permissionId
        })
        .ToList();

        _context.MtRolepermissions.AddRange(permissions);

        _context.SaveChanges();

        response.Result = true;
        response.Message = "Permission updated successfully.";

        return response;
    }
}