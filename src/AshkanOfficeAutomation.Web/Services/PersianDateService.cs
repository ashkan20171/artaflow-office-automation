using System.Globalization;
namespace AshkanOfficeAutomation.Web.Services;
public interface IPersianDateService { string Format(DateTime date,bool withTime=true); }
public class PersianDateService:IPersianDateService {
 readonly PersianCalendar pc=new();
 public string Format(DateTime date,bool withTime=true){
  var d=date.Kind==DateTimeKind.Utc?date.ToLocalTime():date;
  var text=$"{pc.GetYear(d):0000}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}";
  return withTime?$"{text} {d:HH:mm}":text;
 }
}
