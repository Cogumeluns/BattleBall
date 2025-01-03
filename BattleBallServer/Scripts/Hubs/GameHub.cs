using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public FieldInfosDto fieldInfosDto = new();
    public KeyP2InfoDto keyP2InfoDto = new();

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connected!!!");
        return base.OnConnectedAsync();
    }

    // Method to send the class to the client
    public async Task ReceiveFieldInfos()
    {
        // Console.WriteLine("-- ReceiveFieldInfos");
        await Clients.All.SendAsync("ReceiveFieldInfos", fieldInfosDto);
    }

    public async Task ReceiveKeyInfos()
    {
        // Console.WriteLine("-- ReceiveKeyInfos");
        await Clients.All.SendAsync("ReceiveKeyInfos", keyP2InfoDto);
    }

    // Method to receive the class from the client
    public async Task SendInfosToServer(FieldInfosDto fieldInfosDto)
    {
        // Console.WriteLine("-- SendInfosToServer");
        this.fieldInfosDto = fieldInfosDto;
        await ReceiveFieldInfos();
    }

    // Method to receive the class from the client
    public async Task SendKeysToServer(KeyP2InfoDto keyP2InfoDto)
    {
        this.keyP2InfoDto = keyP2InfoDto;
        await ReceiveKeyInfos();
    }

    public async Task ReceiveClientConnectedInfo() =>
        await Clients.All.SendAsync("ReceiveClientConnectedInfo");

    public async Task SendClientConnectedInfo() =>
        await ReceiveClientConnectedInfo();

    public async Task ReceiveOpenLanMode() =>
        await Clients.All.SendAsync("ReceiveOpenLanMode");

    public async Task SendOpenLanMode() =>
        await ReceiveOpenLanMode();

    public async Task ReceiveOpenMainMenu() =>
        await Clients.All.SendAsync("ReceiveOpenMainMenu");

    public async Task SendOpenMainMenu() =>
        await ReceiveOpenMainMenu();
        
}