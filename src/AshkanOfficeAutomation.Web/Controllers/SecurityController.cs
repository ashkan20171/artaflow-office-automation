using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize] public class SecurityController(UserManager<AppUser> users,AppDbContext db):Controller {
 public async Task<IActionResult> Index(){
  var u=await users.GetUserAsync(User);if(u==null)return Challenge();
  ViewBag.TwoFactor=u.TwoFactorEnabled;
  return View(await db.UserSessionRecords.Where(x=>x.UserId==u.Id).OrderByDescending(x=>x.Id).Take(30).ToListAsync());
 }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> EnableAuthenticator(){
  var u=await users.GetUserAsync(User);if(u==null)return Challenge();
  var key=await users.GetAuthenticatorKeyAsync(u);if(string.IsNullOrEmpty(key)){await users.ResetAuthenticatorKeyAsync(u);key=await users.GetAuthenticatorKeyAsync(u);}
  ViewBag.Key=key;TempData["Info"]="کلید Authenticator ایجاد شد. برای فعال‌سازی نهایی باید کد TOTP را تأیید کنید.";return RedirectToAction(nameof(Index));
 }

 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> VerifyAuthenticator(string code){
  var u=await users.GetUserAsync(User);if(u==null)return Challenge();
  code=(code??"").Replace(" ","").Replace("-","");
  if(!await users.VerifyTwoFactorTokenAsync(u,users.Options.Tokens.AuthenticatorTokenProvider,code)){TempData["Error"]="کد واردشده معتبر نیست.";return RedirectToAction(nameof(Index));}
  await users.SetTwoFactorEnabledAsync(u,true);
  var codes=await users.GenerateNewTwoFactorRecoveryCodesAsync(u,8);TempData["RecoveryCodes"]=string.Join(" | ",codes!);return RedirectToAction(nameof(Index));
 }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> DisableTwoFactor(){
  var u=await users.GetUserAsync(User);if(u==null)return Challenge();await users.SetTwoFactorEnabledAsync(u,false);return RedirectToAction(nameof(Index));
 }

 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Revoke(long id){
  var uid=users.GetUserId(User)!;var s=await db.UserSessionRecords.FirstOrDefaultAsync(x=>x.Id==id&&x.UserId==uid);if(s!=null){s.RevokedAt=DateTime.UtcNow;await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));
 }
}
