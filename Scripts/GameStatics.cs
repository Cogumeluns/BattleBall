using BattleBall;
using System.Diagnostics;

public static class GameStatics
{
    public static void Initialize(GameMain gameMain, bool isMaster)
    {
        connection = new Connection(gameMain);
        GameStatics.isMaster = isMaster;
    }

    public static Connection connection = null;
    public static bool isMaster = false;
    public static bool isConnectedClient = false;
    public static string connectionPath = "http://127.0.0.1:5001/GameHub";
    public static Process process = null;
} 