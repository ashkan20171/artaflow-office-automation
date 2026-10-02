using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Services;

public sealed class SlaMonitoringService(IServiceScopeFactory scopes, ILogger<SlaMonitoringService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
        while (!stoppingToken.IsCancellationRequested)
        {
            try {
                await CheckAsync(stoppingToken);
                await timer.WaitForNextTickAsync(stoppingToken);
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
              catch (Exception ex) { logger.LogError(ex,"SLA monitoring cycle failed."); await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken); }
        }
    }

    private async Task CheckAsync(CancellationToken ct)
    {
        using var scope=scopes.CreateScope();
        var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var now=DateTime.Now;
        var due=await db.Letters.AsNoTracking()
            .Where(x=>x.DueAt!=null && x.DueAt<=now.AddHours(4) &&
                x.Status!=LetterStatus.Completed && x.Status!=LetterStatus.Archived)
            .Select(x=>new {x.Id,x.Number,x.Subject,x.CreatorId,x.DueAt}).Take(100).ToListAsync(ct);

        foreach(var x in due) {
            var marker=$"SLA:{x.Id}:{x.DueAt:yyyyMMddHH}";
            var exists=await db.Notifications.AnyAsync(n=>n.UserId==x.CreatorId && n.Message.Contains(marker),ct);
            if(exists) continue;
            db.Notifications.Add(new Notification {
                UserId=x.CreatorId, Title=x.DueAt<now?"هشدار عبور از SLA":"سررسید نزدیک است",
                Message=$"{x.Number} - {x.Subject} | {marker}", Link=$"/Letters/Details/{x.Id}", CreatedAt=DateTime.UtcNow
            });
        }
        if(due.Count>0) await db.SaveChangesAsync(ct);
    }
}
