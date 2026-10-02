using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Services;

public interface IActivityFeedService
{
    Task<ActivityFeedViewModel> GetAsync(int take = 50);
}

public sealed class ActivityFeedService(AppDbContext db) : IActivityFeedService
{
    public async Task<ActivityFeedViewModel> GetAsync(int take = 50)
    {
        var today = DateTime.UtcNow.Date;
        var week = today.AddDays(-7);
        return new ActivityFeedViewModel
        {
            Events = await db.AuditEvents.AsNoTracking()
                .OrderByDescending(x => x.At).Take(Math.Clamp(take, 10, 200)).ToListAsync(),
            TodayCount = await db.AuditEvents.CountAsync(x => x.At >= today),
            WeekCount = await db.AuditEvents.CountAsync(x => x.At >= week)
        };
    }
}
