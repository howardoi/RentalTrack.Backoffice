using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalTrack.Backoffice.Models;
using RentalTrack.Backoffice.Services;
using RentalTrack.Business.Interfaces;
using RentalTrack.Utility.Enums;

namespace RentalTrack.Backoffice.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _auth;
    private readonly IEventLogService _log;

    public AccountController(IAuthService auth, IEventLogService log)
    {
        _auth = auth;
        _log = log;
    }

    [HttpGet, AllowAnonymous]
    public IActionResult Login(string? returnUrl)
        => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _auth.AuthenticateAsync(model.Email, model.Password);
        if (!result.Success || result.Principal is null)
        {
            _log.Append("warn", "auth.login_failed", $"Failed login for {model.Email}", HttpContext.Connection.RemoteIpAddress?.ToString());
            model.ErrorMessage = result.ErrorMessage ?? "Email or password is incorrect.";
            model.Password = string.Empty;
            return View(model);
        }

        var p = result.Principal;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, p.Id.ToString()),
            new(ClaimTypes.Name, p.Email),
            new("DisplayName", p.DisplayName),
            new(ClaimTypes.Role, p.Role == AdminRole.SuperAdmin ? "SuperAdmin" : "Admin"),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        _log.Append("info", "auth.login", $"{p.Email} signed in", HttpContext.Connection.RemoteIpAddress?.ToString());

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
