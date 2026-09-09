using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Controllers;

[Authorize]
public class PermissionController(IPermissionService permissionService) : Controller
{
    private readonly IPermissionService _permissionService = permissionService;

    public IActionResult Index()
    {
        var roles = _permissionService.GetRoles();

        ViewBag.RoleList = new SelectList(
            roles,
            "Id",
            "Name"
        );

        return View();
    }

    [HttpGet]
    public JsonResult GetRolePermissions(int roleId)
    {
        var result = _permissionService.GetRolePermissions(roleId);

        return Json(new
        {
            Success = true,
            Data = result
        });
    }

    [HttpPost]
    public JsonResult Save(SaveRolePermissionRequest request)
    {
        try
        {
            var result = _permissionService.SaveRolePermissions(request);

            if (!result.Result)
            {
                return Json(new
                {
                    Success = false,
                    Message = result.Message
                });
            }

            Cache.ClearPerms(HttpContext);

            return Json(new
            {
                Success = true,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}