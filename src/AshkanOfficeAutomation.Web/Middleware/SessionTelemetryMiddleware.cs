using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Middleware;
public class SessionTelemetryMiddleware(RequestDelegate next) {
 public async Task InvokeAsync(HttpContext ctx,AppDbContext db,UserManager<AppUser> users) {
  if(ctx.User.Identity?.IsAuthenticated==true) {
   var uid=users.GetUserId(ctx.User);
   if(uid!=null) {
    var key=ctx.Request.Cookies["ashkan.sid"];
    if(string.IsNullOrWhiteSpace(key)) {
     key=Guid.NewGuid().ToString("N");
     ctx.Response.Cookies.Append("ashkan.sid",key,new CookieOptions{HttpOnly=true,Secure=ctx.Request.IsHttps,SameSite=SameSiteMode.Lax,IsEssential=true,MaxAge=TimeSpan.FromDays(30)});
    }
    var rec=await db.UserSessionRecords.FirstOrDefaultAsync(x=>x.UserId==uid&&x.SessionKey==key);
    if(rec==null) {
     rec=new UserSessionRecord{UserId=uid,SessionKey=key,IpAddress=ctx.Connection.RemoteIpAddress?.ToString(),UserAgent=ctx.Request.Headers.UserAgent.ToString()};
     db.UserSessionRecords.Add(rec); await db.SaveChangesAsync();
    } else if(rec.RevokedAt!=null) {
     ctx.Response.Cookies.Delete("ashkan.sid");
     ctx.Response.Cookies.Delete(".AspNetCore.Identity.Application");
     ctx.Response.Redirect("/Account/Login"); return;
    } else if(DateTime.UtcNow-rec.LastSeenAt>TimeSpan.FromMinutes(5)) {
     rec.LastSeenAt=DateTime.UtcNow; await db.SaveChangesAsync();
    }
   }
  }
  await next(ctx);
 }
}
