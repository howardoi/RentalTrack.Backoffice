using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalTrack.Business.Interfaces;

namespace RentalTrack.Backoffice.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboard;

    public DashboardController(IDashboardService dashboard) => _dashboard = dashboard;

    public async Task<IActionResult> Index()
    {
        var vm = await _dashboard.GetDashboardAsync(10);
        return View(vm);
    }
}
