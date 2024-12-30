using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall;

public class GamePhysicTest : Game
{
    public Connection Connection = new Connection();
    bool isLan = true;
    bool isMaster = true;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Point GameBounds = new Point(1280, 720);

    private CollisionComponent _collisionComponent;

    List<IUpdateDrawable> updateDrawables = new();
    List<IUpdateDrawable> tempUpdateDrawables = new();
 
    Player player1;
    Player player2;
    Ball ball;
    Field field;

    ControllerBallLight instantiateBallLight;

    public GamePhysicTest()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBounds.X;
        _graphics.PreferredBackBufferHeight = GameBounds.Y;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _collisionComponent = new CollisionComponent(new RectangleF(0, 0, GameBounds.X, GameBounds.Y));
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        player1 = new(new(new(GameBounds.X / 4, GameBounds.Y / 2), 30), Color.Blue);
        var player1ListMovEvents = new Dictionary<PlayerKeys, Func<KeyboardState, bool>>()
        {
            { PlayerKeys.Up, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.W) },
            { PlayerKeys.Down, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.S) },
            { PlayerKeys.Left, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.A) },
            { PlayerKeys.Right, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.D) },
            { PlayerKeys.Dash, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Space) }
        }; 
        player1.SetKeys(player1ListMovEvents);

        player2 = new(new(new(GameBounds.X - (GameBounds.X / 4), GameBounds.Y / 2), 30), Color.Red);
        var player2ListMovEvents = new Dictionary<PlayerKeys, Func<KeyboardState, bool>>();
        if (isLan && isMaster)
        {
            player2ListMovEvents = new Dictionary<PlayerKeys, Func<KeyboardState, bool>>()
            {
                { PlayerKeys.Up, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[0] },
                { PlayerKeys.Down, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[1] },
                { PlayerKeys.Left, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[2] },
                { PlayerKeys.Right, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[3] },
                { PlayerKeys.Dash, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[4] }
            };
        } else {
            player2ListMovEvents = new Dictionary<PlayerKeys, Func<KeyboardState, bool>>()
            {
                { PlayerKeys.Up, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Up) },
                { PlayerKeys.Down, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Down) },
                { PlayerKeys.Left, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Left) },
                { PlayerKeys.Right, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Right) },
                { PlayerKeys.Dash, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.NumPad0) }
            };
        }

        player2.SetKeys(player2ListMovEvents);

        field = new(new(new(50, 50), new(GameBounds.X - 100, GameBounds.Y - 100)), Color.White);

        ball = new(new(new(GameBounds.X / 2, GameBounds.Y / 2 + 250), 10), Color.White);

        instantiateBallLight = new(_collisionComponent, tempUpdateDrawables, field, player1, player2);

        _collisionComponent.Insert(player1);
        _collisionComponent.Insert(player2);
        _collisionComponent.Insert(field);
        _collisionComponent.Insert(ball);
        updateDrawables.Add(player1);
        updateDrawables.Add(player2);
        updateDrawables.Add(field);
        updateDrawables.Add(ball);
        updateDrawables.Add(instantiateBallLight);

        if (isLan)
        {
            Task.Run(
                async () => await Connection.Connect()
            ).Wait();
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (updateDrawables.Count != 0)
        {
            for (int i = 0; i < updateDrawables.Count; i++)
            {
                IBaseDisposable baseDisposable = updateDrawables[i];
                if (baseDisposable.isDisposed)
                {
                    if (updateDrawables[i] is ICollisionActor actor)
                    {
                        _collisionComponent.Remove(actor);
                    }

                    updateDrawables.RemoveAt(i);
                }
            }
        }

        if (tempUpdateDrawables.Count != 0)
        {
            updateDrawables.AddRange(tempUpdateDrawables);
            tempUpdateDrawables.Clear();
        }

        if (isLan && isMaster)
        {
            Connection.FieldInfosDto.player1Pos = new float[] { player1.Bounds.Position.X, player1.Bounds.Position.Y };
            Connection.FieldInfosDto.player2Pos = new float[] { player2.Bounds.Position.X, player2.Bounds.Position.Y };
            Connection.FieldInfosDto.ballPos = new float[] { ball.Bounds.Position.X, ball.Bounds.Position.Y };
            if (instantiateBallLight.ballLight != null)
                Connection.FieldInfosDto.ballLightPos = new float[] { instantiateBallLight.ballLight.Bounds.Position.X,
                    instantiateBallLight.ballLight.Bounds.Position.Y };

            updateDrawables.ForEach(x => x.Update(gameTime));
            Task.Run(async () =>
            {
                await Connection.SendFieldInfosToServer();
            }).Wait();
        }
        else if (isLan && !isMaster)
        {
            player1.Bounds.Position = new Vector2(Connection.FieldInfosDto.player1Pos[0], Connection.FieldInfosDto.player1Pos[1]);
            player2.Bounds.Position = new Vector2(Connection.FieldInfosDto.player2Pos[0], Connection.FieldInfosDto.player2Pos[1]);
            ball.Bounds.Position = new Vector2(Connection.FieldInfosDto.ballPos[0], Connection.FieldInfosDto.ballPos[1]);

            if (instantiateBallLight.ballLight != null)
                instantiateBallLight.ballLight.Bounds.Position = new Vector2(Connection.FieldInfosDto.ballLightPos[0], Connection.FieldInfosDto.ballLightPos[1]);
            else {
                instantiateBallLight.Update(gameTime);
            }

            KeyboardState keyboardState = Keyboard.GetState();
            Connection.KeyP2InfoDto.player2Keys = new bool[]
            {
                keyboardState.IsKeyDown(Keys.Up),
                keyboardState.IsKeyDown(Keys.Down),
                keyboardState.IsKeyDown(Keys.Left),
                keyboardState.IsKeyDown(Keys.Right),
                keyboardState.IsKeyDown(Keys.NumPad0)
            };
            Task.Run(async () => await Connection.SendKeyInfosToServer()).Wait();
        }

        _collisionComponent.Update(gameTime);

        // Connection.SendFieldInfosToServer(FieldInfosDto).RunSynchronously();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        updateDrawables.ForEach(x => x.Draw(_spriteBatch));

        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
