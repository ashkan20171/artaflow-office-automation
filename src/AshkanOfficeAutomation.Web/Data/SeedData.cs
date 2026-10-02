using AshkanOfficeAutomation.Web.Models;
using AshkanOfficeAutomation.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AshkanOfficeAutomation.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        await db.Database.EnsureCreatedAsync();
        await DevelopmentSchemaUpgrader.UpgradeAsync(db);

        foreach (var roleName in new[] { "Administrator", "Manager", "Secretariat", "Employee", "Auditor" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var admin = await userManager.FindByNameAsync("admin");
        if (admin is null)
        {
            admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@local.test",
                FullName = "مدیر سامانه",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin@12345");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Administrator");
        }

        if (!await db.Departments.AnyAsync())
        {
            db.Departments.Add(new Department { Name = "مدیریت مرکزی" });
            await db.SaveChangesAsync();
        }



        // Direct idempotent seed after schema reconciliation. This deliberately avoids
        // EF change tracking during startup for databases created by older stages.
        await db.Database.ExecuteSqlRawAsync("""
IF OBJECT_ID(N'[OrganizationSettings]', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM [OrganizationSettings])
BEGIN
    INSERT INTO [OrganizationSettings]
        ([OrganizationName],[LetterPrefix],[DefaultSlaHours],[SupportEmail],[SupportPhone])
    VALUES
        (N'Ashkan Office Automation',N'ASH',48,NULL,NULL);
END;
""");

        var grants = new Dictionary<string, string[]>
        {
            ["Manager"] = new[]
            {
                Permissions.DashboardView, Permissions.LettersView, Permissions.LettersCreate,
                Permissions.LettersRefer, Permissions.LettersSearch, Permissions.AttachmentsUpload,
                Permissions.ReportsView, Permissions.TasksManage, Permissions.MeetingsManage,
                Permissions.TemplatesManage, Permissions.KnowledgeManage
            },
            ["Secretariat"] = new[]
            {
                Permissions.DashboardView, Permissions.LettersView, Permissions.LettersCreate,
                Permissions.LettersRegister, Permissions.LettersRefer, Permissions.LettersArchive,
                Permissions.LettersSearch, Permissions.AttachmentsUpload, Permissions.TasksManage,
                Permissions.MeetingsManage
            },
            ["Employee"] = new[]
            {
                Permissions.DashboardView, Permissions.LettersView, Permissions.LettersCreate,
                Permissions.LettersRefer, Permissions.LettersSearch, Permissions.AttachmentsUpload,
                Permissions.TasksManage, Permissions.MeetingsManage
            },
            ["Auditor"] = new[]
            {
                Permissions.DashboardView, Permissions.LettersView, Permissions.LettersSearch,
                Permissions.ReportsView, Permissions.AuditView
            }
        };

        foreach (var grant in grants)
        {
            var role = await roleManager.FindByNameAsync(grant.Key);
            if (role is null) continue;

            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in grant.Value)
            {
                if (!claims.Any(c => c.Type == "permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }
        }

        var administratorRole = await roleManager.FindByNameAsync("Administrator");
        if (administratorRole is not null)
        {
            var claims = await roleManager.GetClaimsAsync(administratorRole);
            foreach (var permission in Permissions.All)
            {
                if (!claims.Any(c => c.Type == "permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(administratorRole, new Claim("permission", permission));
            }
        }
    }
}
