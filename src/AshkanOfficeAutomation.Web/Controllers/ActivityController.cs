using AshkanOfficeAutomation.Web.Security;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AshkanOfficeAutomation.Web.Controllers;

[Authorize(Policy = Permissions.AuditView)]
public sealed class ActivityController(IActivityFeedService activity) : Controller
{
    public async Task<IActionResult> Index() => View(await activity.GetAsync());
}
