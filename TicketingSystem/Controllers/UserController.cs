using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketingSystem.Helper;
using TicketingSystem.Models;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Controllers;

[Authorize]
public class UserController(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;

    public IActionResult Index()
    {
        var roles = _userService.GetRoles();

        ViewBag.RoleList = new SelectList(roles, "Id", "Name");

        return View();
    }

    [HttpPost]
    public JsonResult Read(Datatable datatable, UserFilter filters)
    {
        Support.ProccessFilter(datatable, out var col, out var colIndex, out var sort);

        var result = _userService.GetUsers(new BasePaging
        {
            Column = col,
            SortBy = sort,
            Start = datatable.Start,
            Length = datatable.Length
        }, filters);

        return new JsonResult(new
        {
            draw = datatable.Draw,
            data = result.Result,

            recordsTotal = result.Total,
            recordsFiltered = result.TotalFiltered
        });
    }

    [HttpPost]
    public JsonResult Create(CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                Success = false,
                Message = "Fields required!"
            });
        }

        try
        {
            var result = _userService.CreateUser(request);

            return Json(new
            {
                Success = result.Result,
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

    [HttpGet]
    public JsonResult Details(int id)
    {
        try
        {
            var result = _userService.GetDetails(id);

            if (result.Result == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = result.Message
                });
            }

            return Json(new
            {
                Success = true,
                Message = "Success",
                Data = result.Result
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

    [HttpPost]
    public JsonResult Update(UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                Success = false,
                Message = "Fields required!"
            });
        }

        try
        {
            var result = _userService.UpdateUser(request);

            return Json(new
            {
                Success = result.Result,
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

    [HttpPost]
    public JsonResult ToggleActive(int id)
    {
        try
        {
            var result = _userService.ToggleActive(id, User.Id());

            return Json(new
            {
                Success = result.Result,
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