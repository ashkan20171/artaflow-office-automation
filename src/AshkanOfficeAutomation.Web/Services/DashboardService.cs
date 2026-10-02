using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Services;
public interface IDashboardService { Task<DashboardViewModel> BuildAsync(string userId,bool administrator); }
public sealed class DashboardService(AppDbContext db):IDashboardService {
 public async Task<DashboardViewModel> BuildAsync(string uid,bool admin) {
  var letters=db.Letters.AsNoTracking();
  if(!admin) letters=letters.Where(x=>x.CreatorId==uid||x.Referrals.Any(r=>r.ToUserId==uid));
  return new DashboardViewModel {
   TotalLetters=await letters.CountAsync(),
   InWorkflow=await letters.CountAsync(x=>x.Status==LetterStatus.InWorkflow||x.Status==LetterStatus.Registered),
   Completed=await letters.CountAsync(x=>x.Status==LetterStatus.Completed),
   Archived=await letters.CountAsync(x=>x.Status==LetterStatus.Archived),
   MyOpenTasks=await db.WorkItems.CountAsync(x=>x.AssigneeId==uid&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled),
   OverdueTasks=await db.WorkItems.CountAsync(x=>x.AssigneeId==uid&&x.DueAt<DateTime.Now&&x.Status!=WorkItemStatus.Done&&x.Status!=WorkItemStatus.Cancelled),
   UnreadNotifications=await db.Notifications.CountAsync(x=>x.UserId==uid&&x.ReadAt==null),
   RecentLetters=await letters.OrderByDescending(x=>x.Id).Take(8).ToListAsync()
  };
 }
}
