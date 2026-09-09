using Microsoft.Extensions.Caching.Memory;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Helper;

public static class Cache
{
    private const string PermissionCacheKey = "Permission";

    public static List<RequestPermsRoleList> CachePerms(HttpContext httpContext)
    {
        var memoryCache = httpContext.RequestServices.GetRequiredService<IMemoryCache>();

        var permissionService = httpContext.RequestServices .GetRequiredService<IPermissionService>();

        var cache = memoryCache.Get<List<RequestPermsRoleList>>(PermissionCacheKey);

        if (cache != null)
        {
            return cache;
        }

        var permissions = permissionService.GetPermsRole(0);

        memoryCache.Set(PermissionCacheKey, permissions,
            new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(60)
            }
        );

        return permissions;
    }

    public static void ClearPerms(HttpContext httpContext)
    {
        var memoryCache = httpContext.RequestServices.GetRequiredService<IMemoryCache>();

        memoryCache.Remove(PermissionCacheKey);
    }
}