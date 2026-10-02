using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore; using System.Data;
namespace AshkanOfficeAutomation.Web.Services;
public interface IRegistryService { Task<string> NextAsync(LetterType type); }
public class RegistryService(AppDbContext db):IRegistryService {
 public async Task<string> NextAsync(LetterType type){
  var year=DateTime.Now.Year;
  await using var tx=await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
  var seq=await db.RegistrySequences.SingleOrDefaultAsync(x=>x.Year==year&&x.Type==type);
  if(seq==null){seq=new RegistrySequence{Year=year,Type=type,LastNumber=0,Prefix="ASH"};db.RegistrySequences.Add(seq);}
  seq.LastNumber++; await db.SaveChangesAsync(); await tx.CommitAsync();
  var code=type switch{LetterType.Incoming=>"IN",LetterType.Outgoing=>"OUT",_=>"INT"};
  return $"{seq.Prefix}-{code}-{year}-{seq.LastNumber:000000}";
 }
}
