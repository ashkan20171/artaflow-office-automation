using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.SignalR;
namespace AshkanOfficeAutomation.Web.Hubs;
[Authorize] public class NotificationHub:Hub { }
