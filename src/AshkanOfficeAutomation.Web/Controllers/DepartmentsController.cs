using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.DepartmentsManage)]
public class DepartmentsController(AppDbContext db):Controller {
 public async Task<IActionResult> Index()=>View(await db.Departments.Include(x=>x.Parent).OrderBy(x=>x.Name).ToListAsync());
 [HttpGet] public async Task<IActionResult> Create(){ViewBag.Departments=await db.Departments.OrderBy(x=>x.Name).ToListAsync();return View(new DepartmentInput());}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(DepartmentInput input){
  if(input.ParentId.HasValue&&!await db.Departments.AnyAsync(x=>x.Id==input.ParentId))ModelState.AddModelError(nameof(input.ParentId),"واحد والد معتبر نیست.");
  if(!ModelState.IsValid){ViewBag.Departments=await db.Departments.ToListAsync();return View(input);}
  db.Departments.Add(new Department{Name=input.Name,ParentId=input.ParentId});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
}
