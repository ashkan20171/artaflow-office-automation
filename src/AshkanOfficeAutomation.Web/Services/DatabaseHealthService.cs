using AshkanOfficeAutomation.Web.Data;
using AshkanOfficeAutomation.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AshkanOfficeAutomation.Web.Services;

public interface IDatabaseHealthService { Task<DatabaseHealthViewModel> CheckAsync(); }

public sealed class DatabaseHealthService(AppDbContext db) : IDatabaseHealthService
{
    public async Task<DatabaseHealthViewModel> CheckAsync()
    {
        var can=await db.Database.CanConnectAsync();
        var missing=new List<string>();
        if(can && db.Database.IsSqlServer())
        {
            var connection=db.Database.GetDbConnection();
            if(connection.State!=ConnectionState.Open) await connection.OpenAsync();
            foreach(var column in new[]{"OrganizationName","LetterPrefix","DefaultSlaHours","SupportEmail","SupportPhone"})
            {
                await using var cmd=connection.CreateCommand();
                cmd.CommandText="SELECT CASE WHEN COL_LENGTH('OrganizationSettings', @column) IS NULL THEN 0 ELSE 1 END";
                var p=cmd.CreateParameter();p.ParameterName="@column";p.Value=column;cmd.Parameters.Add(p);
                var exists=Convert.ToInt32(await cmd.ExecuteScalarAsync())==1;
                if(!exists) missing.Add(column);
            }
        }
        return new DatabaseHealthViewModel {
            CanConnect=can,
            Provider=db.Database.ProviderName ?? "Unknown",
            Database=db.Database.GetDbConnection().Database,
            MissingOrganizationColumns=missing
        };
    }
}
