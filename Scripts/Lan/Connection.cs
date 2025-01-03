using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Diagnostics;
using BattleBall;

public class Connection
{
    public HubConnection hubConnection;

    public FieldInfosDto FieldInfosDto { get; set; } = new();
    public KeyP2InfoDto KeyP2InfoDto { get; set; } = new();
    public GameMain Game { get; set; }

    public Connection(GameMain gameMain)
    {
        Game = gameMain;
    }

    public async Task<bool> Connect(string localPath = null)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(localPath)
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

        hubConnection.On("ReceiveClientConnectedInfo", () => {
            GameStatics.isConnectedClient = true;
        });

        hubConnection.On("ReceiveOpenLanMode", () => {
            Game.gameSceneManager.LoadScene(global::Scene.GAME_MODE_LAN);
        });

        hubConnection.On("ReceiveOpenMainMenu", () => {
            Game.gameSceneManager.LoadScene(global::Scene.MAIN_MENU);
        });

        await hubConnection.StartAsync();
        Console.WriteLine("Connection established");
        return true;
    }

    public async Task SendFieldInfosToServer()
    {
        await hubConnection.InvokeAsync("SendInfosToServer", FieldInfosDto);
    }

    public async Task SendKeyInfosToServer()
    {
        await hubConnection.InvokeAsync("SendKeysToServer", KeyP2InfoDto);
    }

    public async Task SendClientConnectedInfo()
    {
        await hubConnection.InvokeAsync("SendClientConnectedInfo");
    }

    public async Task SendOpenLanMode()
    {
        await hubConnection.InvokeAsync("SendOpenLanMode");
    }

    public async Task SendOpenMainMenu()
    {
        await hubConnection.InvokeAsync("SendOpenMainMenu");
    }

}