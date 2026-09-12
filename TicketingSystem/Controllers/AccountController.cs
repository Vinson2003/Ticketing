using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Controllers;

public class AccountController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage = "Username and password are required.";

            return View(request);
        }

        var result = _authService.Login(request);

        if (result.Result == null)
        {
            ViewBag.ErrorMessage = result.Message;

            return View(request);
        }

        var user = result.Result;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, user.RoleName),

            new("RoleId", user.RoleId.ToString()),
            new("RoleCode", user.RoleCode),
            new("Username", user.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var result = _authService.GetProfile(User.Id());

        if (result.Result == null)
        {
            return NotFound();
        }

        return View(result.Result);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public JsonResult ChangePassword(ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                Success = false,
                Message = "Please complete all required fields correctly."
            });
        }

        try
        {
            var result = _authService.ChangePassword(User.Id(), request
            );

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
}