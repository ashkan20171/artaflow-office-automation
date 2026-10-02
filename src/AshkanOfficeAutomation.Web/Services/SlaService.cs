using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Services;

public interface ISlaService
{
    Task<SlaDashboardViewModel> GetDashboardAsync();
    Task<int> ApplyDefaultDeadlinesAsync(int defaultHours);
}

public sealed class SlaService(AppDbContext db) : ISlaService
{
    public async Task<SlaDashboardViewModel> GetDashboardAsync()
    {
        var rows = await db.Letters.AsNoTracking()
            .Where(x => x.DueAt != null && x.Status != LetterStatus.Archived)
            .OrderBy(x => x.DueAt).Take(200)
            .Select(x => new SlaItemViewModel {
                LetterId=x.Id, Number=x.Number, Subject=x.Subject, DueAt=x.DueAt,
                Status=x.Status, Priority=x.Priority
            }).ToListAsync();

        return new SlaDashboardViewModel {
            Items=rows,
            Overdue=rows.Count(x=>x.IsOverdue),
            DueSoon=rows.Count(x=>!x.IsOverdue && x.DueAt.HasValue && x.RemainingHours<=24 &&
                x.Status!=LetterStatus.Completed),
            Healthy=rows.Count(x=>!x.IsOverdue && (!x.DueAt.HasValue || x.RemainingHours>24))
        };
    }

    public async Task<int> ApplyDefaultDeadlinesAsync(int defaultHours)
    {
        defaultHours=Math.Clamp(defaultHours,1,720);
        var letters=await db.Letters.Where(x=>x.DueAt==null &&
            x.Status!=LetterStatus.Completed && x.Status!=LetterStatus.Archived).ToListAsync();
        foreach(var x in letters) x.DueAt=x.CreatedAt.AddHours(defaultHours);
        await db.SaveChangesAsync();
        return letters.Count;
    }
}
