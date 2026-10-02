using Microsoft.AspNetCore.Authorization;
namespace AshkanOfficeAutomation.Web.Security;
public class PermissionRequirement(string permission):IAuthorizationRequirement { public string Permission{get;}=permission; }
public class PermissionHandler:AuthorizationHandler<PermissionRequirement>{
 protected override Task HandleRequirementAsync(AuthorizationHandlerContext c,PermissionRequirement r){
  if(c.User.Claims.Any(x=>x.Type=="permission"&&x.Value==r.Permission)||c.User.IsInRole("Administrator")) c.Succeed(r);
  return Task.CompletedTask;
 }}
