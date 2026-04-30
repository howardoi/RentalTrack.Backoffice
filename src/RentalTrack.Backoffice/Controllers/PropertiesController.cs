using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalTrack.Business.Interfaces;

namespace RentalTrack.Backoffice.Controllers;

[Authorize]
public class PropertiesController : Controller
{
    private readonly IPropertyService _properties;

    public PropertiesController(IPropertyService properties) => _properties = properties;

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Search(string? q, string? status, int page = 1, int pageSize = 25)
    {
        var result = await _properties.SearchAsync(q, status, page, pageSize);
        return Json(result);
    }

    [HttpGet("Properties/Detail/{id:long}")]
    public async Task<IActionResult> Detail(long id)
    {
        var detail = await _properties.GetDetailAsync(id);
        if (detail is null) return NotFound();
        return Json(detail);
    }
}
