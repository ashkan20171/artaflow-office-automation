using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Services;
public interface IGlobalSearchService { Task<GlobalSearchViewModel> SearchAsync(string query,string userId,bool admin); }
public sealed class GlobalSearchService(AppDbContext db):IGlobalSearchService
{
 public async Task<GlobalSearchViewModel> SearchAsync(string query,string uid,bool admin)
 {
  query=(query??"").Trim(); if(query.Length<2) return new(){Query=query};
  var letters=db.Letters.AsNoTracking().Where(x=>x.Subject.Contains(query)||x.Number.Contains(query)||x.Body.Contains(query));
  if(!admin) letters=letters.Where(x=>x.CreatorId==uid||x.Referrals.Any(r=>r.ToUserId==uid));
  var tasks=db.WorkItems.AsNoTracking().Where(x=>(x.Title.Contains(query)||(x.Description!=null&&x.Description.Contains(query)))&&(admin||x.AssigneeId==uid||x.CreatorId==uid));
  var articles=db.KnowledgeArticles.AsNoTracking().Where(x=>x.IsPublished&&(x.Title.Contains(query)||x.Content.Contains(query)||(x.Category!=null&&x.Category.Contains(query))||(x.Keywords!=null&&x.Keywords.Contains(query))));
  return new(){Query=query,Letters=await letters.OrderByDescending(x=>x.Id).Take(20).ToListAsync(),Tasks=await tasks.OrderByDescending(x=>x.Id).Take(12).ToListAsync(),Articles=await articles.OrderByDescending(x=>x.Id).Take(12).ToListAsync()};
 }
}
