using System.Text.RegularExpressions;
using AshkanOfficeAutomation.Web.Models;
namespace AshkanOfficeAutomation.Web.Services;
public interface IAiCopilotService { Task<AiCopilotResult> AnalyzeAsync(AiCopilotInput input,CancellationToken ct=default); }
public sealed class LocalAiCopilotService:IAiCopilotService {
 public Task<AiCopilotResult> AnalyzeAsync(AiCopilotInput input,CancellationToken ct=default){
  var text=Regex.Replace(input.Text??"",@"\s+"," ").Trim();
  var words=Regex.Matches(text.ToLowerInvariant(),@"[\p{L}\p{N}]{4,}").Select(x=>x.Value).GroupBy(x=>x).OrderByDescending(x=>x.Count()).Select(x=>x.Key).Take(6).ToArray();
  var mode=(input.Mode??"summary").ToLowerInvariant();
  var result=mode switch {
   "reply"=>new AiCopilotResult{Title="پیش‌نویس پاسخ",Output=$"با سلام و احترام،\n\nپیرو مکاتبه دریافتی درباره «{Short(text,80)}»، موضوع بررسی شد. خواهشمند است در صورت وجود مستندات تکمیلی ارسال فرمایید.\n\nبا احترام",Provider="Local Smart Assistant",Suggestions=new[]{"پیش از ارسال، متن توسط کاربر بازبینی شود."}},
   "classify"=>new AiCopilotResult{Title="تحلیل و طبقه‌بندی",Output=$"اولویت پیشنهادی: {Priority(text)}\nکلیدواژه‌ها: {string.Join("، ",words.DefaultIfEmpty("عمومی"))}",Provider="Local Smart Assistant",Suggestions=new[]{"طبقه‌بندی نهایی نیازمند تأیید انسانی است."}},
   _=>new AiCopilotResult{Title="خلاصه هوشمند",Output=Short(text,520),Provider="Local Smart Assistant",Suggestions=words.Length>0?new[]{"موضوعات پرتکرار: "+string.Join("، ",words)}:Array.Empty<string>()}
  }; return Task.FromResult(result);
 }
 static string Short(string s,int n)=>s.Length<=n?s:s[..n]+"…";
 static string Priority(string s)=>Regex.IsMatch(s,@"فوری|آنی|urgent|immediate",RegexOptions.IgnoreCase)?"فوری":Regex.IsMatch(s,@"مهم|important|deadline|سررسید",RegexOptions.IgnoreCase)?"مهم":"عادی";
}
