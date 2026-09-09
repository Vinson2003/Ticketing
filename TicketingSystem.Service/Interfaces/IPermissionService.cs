using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Service.Interfaces
{
    public interface IPermissionService
    {
        List<RequestPermsRoleList> GetPermsRole(int roleId);

        List<RolePermissionItem> GetRolePermissions(int roleId);

        BaseResponse<bool> SaveRolePermissions(SaveRolePermissionRequest request);

        List<DropdownItem> GetRoles();
    }
}
