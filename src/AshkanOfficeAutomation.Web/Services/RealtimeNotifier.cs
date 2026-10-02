using AshkanOfficeAutomation.Web.Hubs; using Microsoft.AspNetCore.SignalR;
namespace AshkanOfficeAutomation.Web.Services;
public interface IRealtimeNotifier { Task NotifyAsync(string userId,string title,string message,string? link=null); }
public class RealtimeNotifier(IHubContext<NotificationHub> hub):IRealtimeNotifier {
 public Task NotifyAsync(string userId,string title,string message,string? link=null)=>hub.Clients.User(userId).SendAsync("notification",new{title,message,link});
}
