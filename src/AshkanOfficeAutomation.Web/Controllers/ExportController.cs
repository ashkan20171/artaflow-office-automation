using System.Text; using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Security; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize(Policy=Permissions.ReportsView)] public class ExportController(AppDbContext db):Controller {
 public async Task<IActionResult> LettersCsv(){var rows=await db.Letters.AsNoTracking().OrderByDescending(x=>x.Id).Take(5000).ToListAsync();var sb=new StringBuilder();sb.AppendLine("Number,Subject,Type,Status,Priority,CreatedAt,DueAt");foreach(var x in rows){string Q(string? v)=>"\""+(v??"").Replace("\"","\"\"")+"\"";sb.AppendLine($"{Q(x.Number)},{Q(x.Subject)},{x.Type},{x.Status},{x.Priority},{x.CreatedAt:O},{x.DueAt:O}");}return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(),"text/csv","letters.csv");}
}
