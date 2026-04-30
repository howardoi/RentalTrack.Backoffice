using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalTrack.Backoffice.Services;

namespace RentalTrack.Backoffice.Controllers;

[Authorize]
public class LogsController : Controller
{
    private readonly IEventLogService _log;

    public LogsController(IEventLogService log)
    {
        _log = log;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Tail(string? level, string? type, string? q, long? sinceTicks, int limit = 200)
    {
        if (limit < 1 || limit > 1000) limit = 200;
        DateTime? since = sinceTicks.HasValue ? new DateTime(sinceTicks.Value, DateTimeKind.Utc) : null;
        var rows = _log.Query(q, level, type, since, limit);
        return Json(rows);
    }
}
