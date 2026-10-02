using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public sealed class GlobalSearchController(IGlobalSearchService search,UserManager<AppUser> users):Controller
{
 public async Task<IActionResult> Index(string q="")=>View(await search.SearchAsync(q,users.GetUserId(User)!,User.IsInRole("Administrator")));
}
