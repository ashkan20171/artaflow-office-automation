using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models;
namespace AshkanOfficeAutomation.Web.Services;
public interface IAuditService{Task WriteAsync(string user,string action,string entity,string id,string? ip);}
public class AuditService(AppDbContext db):IAuditService{
 public async Task WriteAsync(string u,string a,string e,string id,string? ip){db.AuditEvents.Add(new AuditEvent{UserId=u,Action=a,Entity=e,EntityId=id,Ip=ip});await db.SaveChangesAsync();}
}
