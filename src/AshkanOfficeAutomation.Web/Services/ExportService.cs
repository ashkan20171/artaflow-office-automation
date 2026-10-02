using System.Text;
using AshkanOfficeAutomation.Web.Models;
namespace AshkanOfficeAutomation.Web.Services;
public interface IExportService { byte[] LettersCsv(IEnumerable<Letter> letters); }
public sealed class ExportService:IExportService {
 public byte[] LettersCsv(IEnumerable<Letter> letters) {
  static string E(string? v)=>"\""+(v??"").Replace("\"","\"\"")+"\"";
  var sb=new StringBuilder(); sb.AppendLine("Number,Subject,Type,Priority,Status,CreatedAt");
  foreach(var x in letters) sb.AppendLine($"{E(x.Number)},{E(x.Subject)},{E(x.Type.ToString())},{E(x.Priority.ToString())},{E(x.Status.ToString())},{E(x.CreatedAt.ToString("O"))}");
  return new UTF8Encoding(true).GetBytes(sb.ToString());
 }
}
