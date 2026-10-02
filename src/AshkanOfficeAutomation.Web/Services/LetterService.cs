using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Services;
public interface ILetterService { Task<List<Letter>> LatestAsync(int n=20); Task<Letter> CreateAsync(Letter x); Task<string> NextNumberAsync(); }
public class LetterService(AppDbContext db,IRegistryService registry):ILetterService {
 public Task<List<Letter>> LatestAsync(int n=20)=>db.Letters.AsNoTracking().OrderByDescending(x=>x.Id).Take(n).ToListAsync();
 public Task<string> NextNumberAsync()=>registry.NextAsync(LetterType.Internal);
 public async Task<Letter> CreateAsync(Letter x){if(string.IsNullOrWhiteSpace(x.Number))x.Number=await registry.NextAsync(x.Type); db.Letters.Add(x);await db.SaveChangesAsync();return x;}
}
