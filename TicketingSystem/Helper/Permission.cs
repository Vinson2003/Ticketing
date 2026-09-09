using System.Security.Claims;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Helper;

public static class PermissionHelper
{
    public static bool CheckPerms(ClaimsPrincipal claim, List<RequestPermsRoleList> permissions, string description)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        if (permissions == null || permissions.Count == 0)
        {
            return false;
        }

        var roleId = claim.RoleId();

        return permissions.Any(i =>i.RoleId == roleId && i.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
    }
}