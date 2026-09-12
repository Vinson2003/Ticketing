using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;

namespace TicketingSystem.Controllers;

[Authorize]
public class HomeController(IDashboardService dashboardService) : Controller
{
    private readonly IDashboardService _dashboardService = dashboardService;

    public IActionResult Index()
    {
        var dashboard = _dashboardService.GetDashboard(User.Id(), User.RoleCode());

        return View(dashboard);
    }
}