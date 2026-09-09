using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TicketingSystem.Helper;
using TicketingSystem.Models;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Controllers;

[Authorize]
public class TicketController(ITicketService ticketService, ITicketHistoryService ticketHistoryService) : Controller
{
    private readonly ITicketService _ticketService = ticketService;
    private readonly ITicketHistoryService _ticketHistoryService = ticketHistoryService;

    public IActionResult Index()
    {
        var dropdowns = _ticketService.GetDropdowns();

        ViewBag.CategoryList = new SelectList(dropdowns.Categories, "Id", "Name");
        ViewBag.PriorityList = new SelectList(dropdowns.Priorities, "Id", "Name");
        ViewBag.StatusList = new SelectList(dropdowns.Statuses, "Id", "Name");
        ViewBag.UserList = new SelectList(dropdowns.Users, "Id", "Name");

        return View();
    }

    // POST: Ticket/GetTickets
    [HttpPost]
    public JsonResult GetTickets(Datatable datatable, TicketFilter filters)
    {
        Support.ProccessFilter(datatable, out var col, out var colIndex, out var sort);

        var result = _ticketService.GetTickets(new BasePaging
            {
                Column = col,
                SortBy = sort,
                Start = datatable.Start,
                Length = datatable.Length
            },
            filters
        );

        return new JsonResult(new
        {
            draw = datatable.Draw,
            data = result.Result,

            recordsTotal = result.Total,
            recordsFiltered = result.TotalFiltered
        });
    }

    // GET: Ticket/Details/5
    [HttpGet]
    public JsonResult Details(int id)
    {
        try
        {
            var result = _ticketService.GetDetails(id);

            if (result.Result == null)
            {
                return Json(new
                {
                    Success = false, Message = result.Message
                });
            }

            return Json(new
            {
                Success = true, Message = "Success", Data = result.Result
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                Success = false, Message = ex.Message
            });
        }
    }

    [HttpPost]
    public JsonResult Create(CreateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                Success = false,
                Message = "Fields required!"
            });
        }

        var createdBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var result = _ticketService.CreateTicket(request, createdBy);
            if (!result.Result)
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
    public JsonResult Update(UpdateTicketRequest request)
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
            var result = _ticketService.UpdateTicket(request, User.Id());

            if (!result.Result)
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
    public JsonResult Delete(int id)
    {
        try
        {
            var result = _ticketService.DeleteTicket(id, User.Id());

            if (!result.Result)
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
    public JsonResult GetTicketHistory(int id)
    {
        try
        {
            var result = _ticketHistoryService.GetTicketHistory(id);

            return Json(new
            {
                Success = true,
                Data = result
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