using System.Globalization;
using Microsoft.AspNetCore.Localization;
namespace AshkanOfficeAutomation.Web.Services;
public interface ILocalizedDateService {
 string Date(DateTime value); string Date(DateTime? value);
 string DateTime(DateTime value); string DateTime(DateTime? value); bool IsPersian {get;}
}
public sealed class LocalizedDateService(IHttpContextAccessor http):ILocalizedDateService {
 static readonly PersianCalendar Pc=new();
 public bool IsPersian => (http.HttpContext?.Features.Get<IRequestCultureFeature>()?.RequestCulture.UICulture.Name ?? CultureInfo.CurrentUICulture.Name).StartsWith("fa",StringComparison.OrdinalIgnoreCase);
 public string Date(DateTime v)=>IsPersian?Persian(v,false):v.ToString("yyyy/MM/dd",CultureInfo.InvariantCulture);
 public string Date(DateTime? v)=>v.HasValue?Date(v.Value):"—";
 public string DateTime(DateTime v)=>IsPersian?Persian(v,true):v.ToString("yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture);
 public string DateTime(DateTime? v)=>v.HasValue?DateTime(v.Value):"—";
 static string Persian(DateTime d,bool withTime){var s=$"{Pc.GetYear(d):0000}/{Pc.GetMonth(d):00}/{Pc.GetDayOfMonth(d):00}";return withTime?$"{s} {d:HH:mm}":s;}
}
