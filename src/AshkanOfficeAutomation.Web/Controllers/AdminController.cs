using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class AdminController(AppDbContext db,UserManager<AppUser> users,RoleManager<IdentityRole> roles):Controller{
 [Authorize(Policy=Permissions.UsersManage)] public async Task<IActionResult> Users()=>View(await db.Users.AsNoTracking().OrderBy(x=>x.FullName).ToListAsync());
 [Authorize(Policy=Permissions.UsersManage),HttpGet] public async Task<IActionResult> CreateUser(){
  ViewBag.Roles=await roles.Roles.Select(x=>x.Name).ToListAsync();
  ViewBag.Departments=await db.Departments.ToListAsync();
  return View(new NewUserInput());
 }
 [Authorize(Policy=Permissions.UsersManage),HttpPost,ValidateAntiForgeryToken]
 public async Task<IActionResult> CreateUser(NewUserInput input){
  if(!await roles.RoleExistsAsync(input.Role))ModelState.AddModelError(nameof(input.Role),"نقش معتبر نیست.");
  if(input.DepartmentId.HasValue&&!await db.Departments.AnyAsync(x=>x.Id==input.DepartmentId.Value))ModelState.AddModelError(nameof(input.DepartmentId),"واحد معتبر نیست.");
  if(ModelState.IsValid){
   var u=new AppUser{UserName=input.UserName,Email=input.Email,FullName=input.FullName,DepartmentId=input.DepartmentId,IsActive=true};
   var result=await users.CreateAsync(u,input.Password);
   if(result.Succeeded){await users.AddToRoleAsync(u,input.Role);return RedirectToAction(nameof(Users));}
   foreach(var e in result.Errors)ModelState.AddModelError("",e.Description);
  }
  ViewBag.Roles=await roles.Roles.Select(x=>x.Name).ToListAsync();ViewBag.Departments=await db.Departments.ToListAsync();return View(input);
 }

 [Authorize(Policy=Permissions.UsersManage),HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> ToggleUser(string id){var u=await users.FindByIdAsync(id);if(u==null)return NotFound();if(u.UserName=="admin")return BadRequest("حساب مدیر اصلی قابل غیرفعال‌سازی نیست.");u.IsActive=!u.IsActive;await users.UpdateAsync(u);return RedirectToAction(nameof(Users));}
 [Authorize(Policy=Permissions.AuditView)] public async Task<IActionResult> Audit()=>View(await db.AuditEvents.AsNoTracking().OrderByDescending(x=>x.Id).Take(500).ToListAsync());
 [Authorize(Policy=Permissions.ReportsView)] public IActionResult Reports()=>View();
}
