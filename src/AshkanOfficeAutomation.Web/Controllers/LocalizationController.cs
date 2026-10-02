using AshkanOfficeAutomation.Web.Services; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public sealed class LocalizationController(ILocalizedDateService dates):Controller {
 public IActionResult Index(){ViewBag.Now=dates.DateTime(DateTime.Now);ViewBag.Today=dates.Date(DateTime.Today);return View();}
}
