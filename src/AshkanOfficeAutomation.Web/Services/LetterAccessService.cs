using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AshkanOfficeAutomation.Web.Services;

public interface ILetterAccessService
{
    Task<bool> CanReadAsync(ClaimsPrincipal principal, Letter letter);
    Task<IQueryable<Letter>> ApplyReadableScopeAsync(ClaimsPrincipal principal, IQueryable<Letter> query);
}

public sealed class LetterAccessService(AppDbContext db, UserManager<AppUser> users) : ILetterAccessService
{
    public async Task<bool> CanReadAsync(ClaimsPrincipal principal, Letter letter)
    {
        if (principal.IsInRole("Administrator")) return true;
        var userId = users.GetUserId(principal);
        if (string.IsNullOrWhiteSpace(userId)) return false;
        if (letter.CreatorId == userId || letter.Referrals.Any(x => x.ToUserId == userId)) return true;

        var delegatedOwners = await db.Delegations
            .Where(x => x.DelegateUserId == userId && x.IsActive && x.From <= DateTime.Now && x.To >= DateTime.Now)
            .Select(x => x.OwnerUserId).ToListAsync();

        return delegatedOwners.Contains(letter.CreatorId) ||
               letter.Referrals.Any(x => delegatedOwners.Contains(x.ToUserId));
    }

    public async Task<IQueryable<Letter>> ApplyReadableScopeAsync(ClaimsPrincipal principal, IQueryable<Letter> query)
    {
        if (principal.IsInRole("Administrator")) return query;
        var userId = users.GetUserId(principal);
        if (string.IsNullOrWhiteSpace(userId)) return query.Where(_ => false);

        var delegatedOwners = await db.Delegations
            .Where(x => x.DelegateUserId == userId && x.IsActive && x.From <= DateTime.Now && x.To >= DateTime.Now)
            .Select(x => x.OwnerUserId).ToListAsync();

        return query.Where(x => x.CreatorId == userId ||
            x.Referrals.Any(r => r.ToUserId == userId) ||
            delegatedOwners.Contains(x.CreatorId) ||
            x.Referrals.Any(r => delegatedOwners.Contains(r.ToUserId)));
    }
}
