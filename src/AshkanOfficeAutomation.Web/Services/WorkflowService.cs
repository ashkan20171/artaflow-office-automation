using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Services;

public interface IWorkflowService
{
    Task<(bool Ok,string Message)> TransitionAsync(long letterId, string userId, string action, string? note);
}

public sealed class WorkflowService(AppDbContext db, IAuditService audit) : IWorkflowService
{
    public async Task<(bool Ok,string Message)> TransitionAsync(long letterId,string userId,string action,string? note)
    {
        var letter=await db.Letters.FirstOrDefaultAsync(x=>x.Id==letterId);
        if(letter is null) return (false,"نامه پیدا نشد.");

        var normalized=(action??"").Trim().ToLowerInvariant();
        var allowed = normalized switch {
            "submit" => letter.Status==LetterStatus.Draft,
            "approve" => letter.Status==LetterStatus.InWorkflow || letter.Status==LetterStatus.Registered,
            "reject" => letter.Status==LetterStatus.InWorkflow || letter.Status==LetterStatus.Registered,
            "archive" => letter.Status==LetterStatus.Completed,
            "reopen" => letter.Status==LetterStatus.Completed || letter.Status==LetterStatus.Archived,
            _ => false
        };
        if(!allowed) return (false,"این انتقال با وضعیت فعلی نامه مجاز نیست.");

        switch(normalized) {
            case "submit": letter.Status=LetterStatus.InWorkflow; break;
            case "approve": letter.Status=LetterStatus.Completed; break;
            case "reject": letter.Status=LetterStatus.Draft; break;
            case "archive":
                letter.Status=LetterStatus.Archived; letter.ArchivedAt=DateTime.UtcNow;
                letter.ArchiveCode ??=$"ARC-{DateTime.UtcNow:yyyy}-{letter.Id:000000}";
                break;
            case "reopen": letter.Status=LetterStatus.InWorkflow; letter.ArchivedAt=null; break;
        }

        db.WorkflowActions.Add(new WorkflowAction { LetterId=letter.Id,UserId=userId,Action=normalized,Note=note,At=DateTime.UtcNow });
        await db.SaveChangesAsync();
        await audit.WriteAsync(userId, $"workflow.{normalized}", "Letter", letter.Id.ToString(), note);
        return (true,"عملیات گردش کار انجام شد.");
    }
}
