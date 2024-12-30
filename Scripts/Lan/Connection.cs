using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Diagnostics;

public class Connection
{
    public HubConnection hubConnection;

    public FieldInfosDto FieldInfosDto { get; set; } = new();
    public KeyP2InfoDto KeyP2InfoDto { get; set; } = new();

    public async Task Connect()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/GameHub")
            .Build();

        // Handle the class received from the server
        hubConnection.On<FieldInfosDto>("ReceiveFieldInfos", fieldInfos =>
        {
            this.FieldInfosDto = fieldInfos;
        });

        hubConnection.On<KeyP2InfoDto>("ReceiveKeyInfos", keyP2Info =>
        {
            this.KeyP2InfoDto = keyP2Info;
        });

        await hubConnection.StartAsync();
        Console.WriteLine("Connection established");
        Console.ReadLine();
    }

    public async Task SendFieldInfosToServer()
    {
        await hubConnection.InvokeAsync("SendInfosToServer", FieldInfosDto);
    }

    public async Task SendKeyInfosToServer()
    {
        await hubConnection.InvokeAsync("SendKeysToServer", KeyP2InfoDto);
    }
}