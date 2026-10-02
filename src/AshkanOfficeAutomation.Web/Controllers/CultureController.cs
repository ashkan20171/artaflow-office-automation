using Microsoft.AspNetCore.Localization; using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
public sealed class CultureController:Controller {
 [HttpPost,ValidateAntiForgeryToken] public IActionResult Set(string culture,string? returnUrl){
  culture=culture=="en"?"en":"fa";
  Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
   new CookieOptions{Expires=DateTimeOffset.UtcNow.AddYears(1),IsEssential=true,SameSite=SameSiteMode.Lax});
  return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl)||!Url.IsLocalUrl(returnUrl)?"/":returnUrl);
 }
}
