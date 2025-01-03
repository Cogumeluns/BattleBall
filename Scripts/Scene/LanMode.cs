using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BattleBall;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

public class LanMode : GameScreen
{
    public new GameMain Game => (GameMain)base.Game;
    public LanMode(GameMain game, bool isMaster) : base(game)
    {
        GameStatics.Initialize(game, isMaster);
        if (isMaster)
        {
            RunServer();
        }
    }

    List<IUpdateDrawable> _elements = new();
    EventLanMode eventLanMode;
    List<Text> connecting = new();
    InputField ipInputField;

    public void RunServer()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        string serverPath = isWindows ? Path.Combine(baseDir, "server", "BattleBallServer.exe") :
                Path.Combine(baseDir, "server", "BattleBallServer");

        string args = @"--urls ""http://127.0.0.1:5001""";

        // Start the process
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = serverPath,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        GameStatics.process = Process.Start(startInfo);
        Task.Run(async () => {
            await GameStatics.connection.Connect(GameStatics.connectionPath);
        });
    }

    public override void LoadContent()
    {
        eventLanMode = new(this);

        SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");
        SpriteFont montserratRegular = Content.Load<SpriteFont>("fonts/montserratregular");

        Texture2D button = Content.Load<Texture2D>("textures/button");
        Texture2D cicle = Content.Load<Texture2D>("textures/about");
        Texture2D rectangle = Content.Load<Texture2D>("textures/rectangle");

        Keys[] p1 = { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space };
        Keys[] p2 = { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad0 };

        _elements.AddRange(new List<IUpdateDrawable>()
        {
            new Image(cicle, new Color(81, 125, 201), new Rectangle(310, 80, 160, 160)),
            new Text(montserratBold, p1[0].ToString(), Color.White, 1f, true, new Vector2(365, 300)),
            new Text(montserratBold, p1[1].ToString(), Color.White, 1f, true, new Vector2(365, 370)),
            new Text(montserratBold, p1[2].ToString(), Color.White, 1f, true, new Vector2(365, 440)),
            new Text(montserratBold, p1[3].ToString(), Color.White, 1f, true, new Vector2(365, 510)),
            new Text(montserratBold, p1[4].ToString(), Color.White, 1f, true, new Vector2(365, 580)),

            new Text(montserratBold, "Up", Color.White, 1f, true, new Vector2(720, 300)),
            new Text(montserratBold, "Down", Color.White, 1f, true, new Vector2(700, 370)),
            new Text(montserratBold, "Left", Color.White, 1f, true, new Vector2(715, 440)),
            new Text(montserratBold, "Right", Color.White, 1f, true, new Vector2(700, 510)),
            new Text(montserratBold, "Dash", Color.White, 1f, true, new Vector2(700, 580)),

            new Image(cicle, new Color(255, 106, 106), new Rectangle(1003, 80, 160, 160)),
            new Text(montserratBold, p2[0].ToString(), Color.White, 1f, true, new(1058, 300)),
            new Text(montserratBold, p2[1].ToString(), Color.White, 1f, true, new(1058, 370)),
            new Text(montserratBold, p2[2].ToString(), Color.White, 1f, true, new(1058, 440)),
            new Text(montserratBold, p2[3].ToString(), Color.White, 1f, true, new(1058, 510)),
            new Text(montserratBold, p2[4].ToString(), Color.White, 1f, true, new(1058, 580)),
        });

        if (GameStatics.isMaster)
            OnLoadContentMaster(button, montserratRegular, montserratBold);
        else
            OnLoadContentClient(button, montserratRegular, montserratBold);
        base.LoadContent();
    }


    private void OnLoadContentClient(Texture2D button, SpriteFont montserratRegular, SpriteFont montserratBold)
    {
        ipInputField = new InputField(new Button(new Image(button, Color.White, new Rectangle(500, 773, 440, 78))),
            new Text(montserratRegular, "127.0.0.1", Color.Black, 1f, true), OnIpEndEdit);
        _elements.Add(ipInputField);

        int xFailure = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Failure").X) / 2);
        int xConnect = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Connected").X) / 2);

        connecting.AddRange(new List<Text>()
        {
            new Text(montserratBold, "Failure", Color.Red, 1f, true, new(xFailure, 869)),
            new Text(montserratBold, "Connected", Color.Green, 1f, true, new(xConnect, 869)),
        });

        _elements.AddRange(connecting);

        void OnIpEndEdit(object sender, EventArgs e)
        {
            Task.Run(async () => {
                GameStatics.isConnectedClient = await GameStatics.connection.Connect($"http://{ipInputField.text.text}:5001/GameHub");
                await GameStatics.connection.SendClientConnectedInfo();
            });
        }
    }

    private void OnLoadContentMaster(Texture2D button, SpriteFont montserratRegular, SpriteFont montserratBold)
    {
        _elements.Add(new Button(new Image(button, Color.White, new Rectangle(1083, 900, 310, 80)), eventLanMode.OnStartGameMode, new Text(montserratBold, "START", Color.Black, 1f, true)));

        int xWainting = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Waiting...").X) / 2);
        int xConnect = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Connected").X) / 2);

        connecting.AddRange(new List<Text>()
        {
            new Text(montserratBold, "Waiting...", new(95,95,95), 1f, true, new(xWainting, 869)),
            new Text(montserratBold, "Connected", Color.Green, 1f, true, new(xConnect, 869)),
        });

        _elements.AddRange(connecting);
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Update(GameTime gameTime)
    {
        ChangeTextConnectingActive();

        _elements.ForEach(x => x.Update(gameTime));
    }

    private void ChangeTextConnectingActive()
    {
        if (GameStatics.isConnectedClient)
        {
            connecting[1].isVisible = true;
            connecting[0].isVisible = false;
        }
        else
        {
            connecting[0].isVisible = true;
            connecting[1].isVisible = false;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Black);
        Game.SpriteBatch.Begin();
        _elements.ForEach(x =>
        {
            if (!x.isVisible) return;
            x.Draw(Game.SpriteBatch);
        });
        Game.SpriteBatch.End();
    }
}