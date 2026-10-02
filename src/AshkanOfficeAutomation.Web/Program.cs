using Microsoft.AspNetCore.Localization; using System.Globalization;
using AshkanOfficeAutomation.Web.Data; using AshkanOfficeAutomation.Web.Models; using AshkanOfficeAutomation.Web.Security; using AshkanOfficeAutomation.Web.Services;
using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Identity; using Microsoft.EntityFrameworkCore;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser,IdentityRole>(o=>{o.Password.RequiredLength=8;o.Lockout.MaxFailedAccessAttempts=5;}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(o=>{o.LoginPath="/Account/Login";o.AccessDeniedPath="/Account/Denied";o.Cookie.HttpOnly=true;o.Cookie.SecurePolicy=CookieSecurePolicy.SameAsRequest;});
builder.Services.AddControllersWithViews(); builder.Services.AddSignalR(); builder.Services.AddHttpContextAccessor();
// Application services
builder.Services.AddSingleton<IPersianDateService, PersianDateService>();
builder.Services.AddScoped<IRealtimeNotifier, RealtimeNotifier>();
builder.Services.AddScoped<IRegistryService, RegistryService>();
builder.Services.AddScoped<ILetterService, LetterService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IAiCopilotService, LocalAiCopilotService>();
builder.Services.AddScoped<ILocalizedDateService, LocalizedDateService>();
builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>();
builder.Services.AddScoped<ISlaService, SlaService>();
builder.Services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();
builder.Services.AddScoped<IActivityFeedService, ActivityFeedService>();
builder.Services.AddScoped<ILetterAccessService, LetterAccessService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddHostedService<SlaMonitoringService>();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
foreach(var p in Permissions.All) builder.Services.AddAuthorization(o=>o.AddPolicy(p,x=>x.Requirements.Add(new PermissionRequirement(p))));
var app=builder.Build();
var supportedCultures=new[]{new CultureInfo("fa"),new CultureInfo("en")};
var localizationOptions=new RequestLocalizationOptions{
 DefaultRequestCulture=new RequestCulture("fa"),
 SupportedCultures=supportedCultures,
 SupportedUICultures=supportedCultures
};
localizationOptions.RequestCultureProviders=new IRequestCultureProvider[]{
 new CookieRequestCultureProvider(),
 new AcceptLanguageHeaderRequestCultureProvider()
};
app.UseRequestLocalization(localizationOptions); if(!app.Environment.IsDevelopment()){app.UseExceptionHandler("/Home/Error");app.UseHsts();}
app.Use(async (ctx,next)=>{ctx.Response.Headers["X-Content-Type-Options"]="nosniff";ctx.Response.Headers["X-Frame-Options"]="DENY";ctx.Response.Headers["Referrer-Policy"]="strict-origin-when-cross-origin";await next();});
app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseAuthentication(); app.UseAuthorization();
app.UseMiddleware<AshkanOfficeAutomation.Web.Middleware.SessionTelemetryMiddleware>();
app.MapHub<AshkanOfficeAutomation.Web.Hubs.NotificationHub>("/hubs/notifications");
app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");
await SeedData.InitializeAsync(app.Services); app.Run();
