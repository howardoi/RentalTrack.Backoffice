using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalTrack.Backoffice.Services;
using RentalTrack.Business.Interfaces;

namespace RentalTrack.Backoffice.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserService _users;
    private readonly IEventLogService _log;

    public UsersController(IUserService users, IEventLogService log)
    {
        _users = users;
        _log = log;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Search(string? q, string? status, string? provider, int page = 1, int pageSize = 25)
    {
        var result = await _users.SearchAsync(q, status, provider, page, pageSize);
        return Json(result);
    }

    [HttpGet("Users/Detail/{id:long}")]
    public async Task<IActionResult> Detail(long id)
    {
        var detail = await _users.GetDetailAsync(id);
        if (detail is null) return NotFound();
        return Json(detail);
    }

    [HttpPost("Users/Suspend/{id:long}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Suspend(long id)
    {
        await _users.SuspendAsync(id);
        _log.Append("warn", "user.suspended", $"User {id} suspended by {User.Identity?.Name}", HttpContext.Connection.RemoteIpAddress?.ToString());
        return Ok();
    }
}
