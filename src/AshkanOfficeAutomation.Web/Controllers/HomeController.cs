using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public class HomeController(IDashboardService dashboard,UserManager<AppUser> users):Controller {
 public async Task<IActionResult> Index(){
  var uid=users.GetUserId(User)!;
  return View(await dashboard.BuildAsync(uid,User.IsInRole("Administrator")));
 }
 [AllowAnonymous] public IActionResult Error()=>View();
}
