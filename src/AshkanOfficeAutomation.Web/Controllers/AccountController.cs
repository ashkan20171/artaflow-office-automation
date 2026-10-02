using AshkanOfficeAutomation.Web.Models; using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
public class AccountController(SignInManager<AppUser> sm):Controller{
 [HttpGet] public IActionResult Login()=>View();
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Login(string username,string password,bool remember=false){
  var r=await sm.PasswordSignInAsync(username,password,remember,true); if(r.Succeeded)return RedirectToAction("Index","Home");ViewBag.Error="نام کاربری یا رمز عبور صحیح نیست.";return View();}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Logout(){await sm.SignOutAsync();return RedirectToAction("Login");}
 public IActionResult Denied()=>View();
}
