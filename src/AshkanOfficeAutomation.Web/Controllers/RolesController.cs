using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Security;
using AppPermissions = AshkanOfficeAutomation.Web.Security.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=AppPermissions.RolesManage)]
public class RolesController(RoleManager<IdentityRole> roles):Controller {
 public IActionResult Index()=>View(roles.Roles.OrderBy(x=>x.Name).ToList());
 [HttpGet] public async Task<IActionResult> Permissions(string id){
  var role=await roles.FindByIdAsync(id); if(role==null)return NotFound();
  var claims=await roles.GetClaimsAsync(role);
  ViewBag.All=AppPermissions.All;
  return View(new RolePermissionsInput{RoleName=role.Name!,Permissions=claims.Where(x=>x.Type=="permission").Select(x=>x.Value).ToList()});
 }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Permissions(string id,RolePermissionsInput input){
  var role=await roles.FindByIdAsync(id); if(role==null)return NotFound();
  var allowed=AppPermissions.All.ToHashSet();
  var requested=(input.Permissions??new()).Where(allowed.Contains).Distinct().ToHashSet();
  var current=(await roles.GetClaimsAsync(role)).Where(x=>x.Type=="permission").ToList();
  foreach(var c in current.Where(x=>!requested.Contains(x.Value))) await roles.RemoveClaimAsync(role,c);
  foreach(var p in requested.Where(x=>!current.Any(c=>c.Value==x))) await roles.AddClaimAsync(role,new Claim("permission",p));
  return RedirectToAction(nameof(Index));
 }
}
