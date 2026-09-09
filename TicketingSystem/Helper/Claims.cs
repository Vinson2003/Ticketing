using System.Security.Claims;

namespace TicketingSystem.Helper;

public static class Claims
{
    public static int Id(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return 0;
        }

        var id = claim.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(id, out var result) ? result : 0;
    }

    public static string Name(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return "";
        }

        return claim.FindFirstValue(ClaimTypes.Name) ?? "";
    }

    public static string Email(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return "";
        }

        return claim.FindFirstValue(ClaimTypes.Email) ?? "";
    }

    public static string Role(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return "";
        }

        return claim.FindFirstValue(ClaimTypes.Role) ?? "";
    }

    public static int RoleId(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return 0;
        }

        var roleId = claim.FindFirstValue("RoleId");

        return int.TryParse(roleId, out var result) ? result : 0;
    }

    public static string RoleCode(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return "";
        }

        return claim.FindFirstValue("RoleCode") ?? "";
    }

    public static string Username(this ClaimsPrincipal claim)
    {
        if (claim?.Identity?.IsAuthenticated != true)
        {
            return "";
        }

        return claim.FindFirstValue("Username") ?? "";
    }
}