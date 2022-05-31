using Microsoft.AspNetCore.SignalR;

namespace Fast.ML.Hubs;

public class PageUpdateHub : Hub
{
    private const string GroupName = "fast_ml_web_app";

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message, string operation)
    {
        switch (operation)
        {
            case "train":
                await Clients.All.SendAsync("ReceiveTrainEnd", message);
                break;
            default:
                await Clients.All.SendAsync("ReceivePredictions", message);
                break;
        }
    }
}