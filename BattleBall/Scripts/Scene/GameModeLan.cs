using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;

namespace BattleBall.Scripts.Scene
{
    public class GameModeLan : GameScreen
    {
        public new GameMain Game => (GameMain)base.Game;
        public GameModeLan(GameMain game) : base(game) {
            Connection.FieldInfosDto = new();
        }

        public bool isDisposed { get; private set; }

        public Connection Connection => GameStatics.connection;


        private CollisionComponent _collisionComponent;

        List<IUpdateDrawable> _elements = new();
        List<IUpdateDrawable> _tempElements = new();

        EventGameModeLan eventGameModeLan;

        Player player1;
        Player player2;
        Ball ball;
        Field field;
        ControllerBallLight controllerBallLight;
        Timer timer;

        bool finished = false;

        public override void LoadContent()
        {
            float heigth = Game._graphics.PreferredBackBufferHeight;
            float width = Game._graphics.PreferredBackBufferWidth;

            _collisionComponent = new CollisionComponent(new Rectangle(0, 0, (int)width, (int)heigth));
            eventGameModeLan = new(this);

            player1 = new(new(new(width / 4, heigth / 2), 30), new(81, 125, 201), true);
            player1.SetKeys(new Dictionary<PlayerKeys, Func<KeyboardState, bool>>() {
                    { PlayerKeys.Up, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.W) },
                    { PlayerKeys.Down, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.S) },
                    { PlayerKeys.Left, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.A) },
                    { PlayerKeys.Right, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.D) },
                    { PlayerKeys.Dash, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Space) }
                });

            player2 = new(new(new(width - (width / 4), heigth / 2), 30), new(255, 106, 106), false);
            Dictionary<PlayerKeys, Func<KeyboardState, bool>> player2ListMovEvents = new();
            if (GameStatics.isMaster)
            {
                player2ListMovEvents = new Dictionary<PlayerKeys, Func<KeyboardState, bool>>()
                {
                    { PlayerKeys.Up, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[0] },
                    { PlayerKeys.Down, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[1] },
                    { PlayerKeys.Left, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[2] },
                    { PlayerKeys.Right, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[3] },
                    { PlayerKeys.Dash, (KeyboardState keyboardState) => Connection.KeyP2InfoDto.player2Keys[4] }
                };
            }
            else
            {
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

            field = new(new(new(50, 50), new(width - 100, heigth - 100)), Color.White);

            ball = new(new(new(width / 2, heigth / 2), 10), Color.White);

            controllerBallLight = new(_collisionComponent, _tempElements, field, player1, player2);

            timer = new Timer(new Text(Content.Load<SpriteFont>("fonts/montserratbold"), "", Color.White, 1f, true, new Vector2(width / 2, 5)), player1, player2);

            _collisionComponent.Insert(player1);
            _collisionComponent.Insert(player2);
            _collisionComponent.Insert(field);
            _collisionComponent.Insert(ball);
            _elements.Add(timer);
            _elements.Add(player1);
            _elements.Add(player2);
            _elements.Add(field);
            _elements.Add(ball);
            _elements.Add(controllerBallLight);

            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
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

        public override void Update(GameTime gameTime)
        {
            FinishedMode();

            DisposeElement();

            AddElement();

            if (GameStatics.isMaster)
            {
                _collisionComponent.Update(gameTime);

                _elements.ForEach(x =>
                {
                    if (!x.isVisible) return;
                    x.Update(gameTime);
                });

                Connection.FieldInfosDto.player1Pos = new float[] { player1.Bounds.Position.X, player1.Bounds.Position.Y };
                Connection.FieldInfosDto.player1Lives = player1.Lives;
                Connection.FieldInfosDto.player1Dash = player1.timeWaitDash;
                Connection.FieldInfosDto.player2Pos = new float[] { player2.Bounds.Position.X, player2.Bounds.Position.Y };
                Connection.FieldInfosDto.player2Lives = player2.Lives;
                Connection.FieldInfosDto.player2Dash = player2.timeWaitDash;
                Connection.FieldInfosDto.ballPos = new float[] { ball.Bounds.Position.X, ball.Bounds.Position.Y };
                Connection.FieldInfosDto.ballColor = new int[] { ball.color.R, ball.color.G, ball.color.B };
                if (controllerBallLight.ballLight != null) {
                    Connection.FieldInfosDto.ballLightPos = new float[] {
                        controllerBallLight.ballLight.Bounds.Position.X,
                        controllerBallLight.ballLight.Bounds.Position.Y
                    };
                }
                Connection.FieldInfosDto.time = timer.time;
                Task.Run(async () => {
                    await Connection.SendFieldInfosToServer();
                }).Wait();
            } else {
                if (player1 == null || player2 == null || ball == null) return;
                player1.Bounds.Position = new Vector2(Connection.FieldInfosDto.player1Pos[0], Connection.FieldInfosDto.player1Pos[1]);
                player1.Damage(player1.Lives - Connection.FieldInfosDto.player1Lives);
                player1.timeWaitDash = Connection.FieldInfosDto.player1Dash;
                player2.Bounds.Position = new Vector2(Connection.FieldInfosDto.player2Pos[0], Connection.FieldInfosDto.player2Pos[1]);
                player2.Damage(player2.Lives - Connection.FieldInfosDto.player2Lives);
                player2.timeWaitDash = Connection.FieldInfosDto.player2Dash;
                ball.Bounds.Position = new Vector2(Connection.FieldInfosDto.ballPos[0], Connection.FieldInfosDto.ballPos[1]);
                ball.color = new Color(
                    Connection.FieldInfosDto.ballColor[0], 
                    Connection.FieldInfosDto.ballColor[1], 
                    Connection.FieldInfosDto.ballColor[2]
                );
                timer.time = Connection.FieldInfosDto.time;

                if (controllerBallLight.ballLight != null) { 
                    controllerBallLight.ballLight.Bounds.Position = new Vector2(
                        Connection.FieldInfosDto.ballLightPos[0], 
                        Connection.FieldInfosDto.ballLightPos[1]
                    );
                } else {
                    controllerBallLight.Update(gameTime);
                }

                KeyboardState keyboardState = Keyboard.GetState();
                Connection.KeyP2InfoDto.player2Keys = new bool[] {
                    keyboardState.IsKeyDown(Keys.Up),
                    keyboardState.IsKeyDown(Keys.Down),
                    keyboardState.IsKeyDown(Keys.Left),
                    keyboardState.IsKeyDown(Keys.Right),
                    keyboardState.IsKeyDown(Keys.NumPad0)
                };
                Task.Run(async () => await Connection.SendKeyInfosToServer()).Wait();
            }
        }

        private void FinishedMode()
        {
            if (finished) return;
            if (timer.IsFinished || player1.Lives == 0 || player2.Lives == 0)
            {

                SpriteFont modak = Content.Load<SpriteFont>("fonts/modak");
                SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");

                Texture2D button = Content.Load<Texture2D>("textures/button");

                Color color;
                string text = "WINNER";
                int width = Game._graphics.PreferredBackBufferWidth;

                if (player1.Lives != 0)
                    color = player1.color;
                else if (player2.Lives != 0)
                    color = player2.color;
                else
                {
                    color = Color.White;
                    text = "DRAW";
                }
                _tempElements.Add(new Text(modak, text, color, 1f, true, new((int)((width - modak.MeasureString(text).X)/2), 100)));
                if (GameStatics.isMaster)
                {
                    _tempElements.AddRange(new List<IUpdateDrawable>()
                    {
                        new Button(new Image(button, Color.White, new Rectangle(187, 780, 310, 80)), eventGameModeLan.OnPlayAgain, new Text(montserratBold, "Play Again", Color.Black, 1f, true)),
                        new Button(new Image(button, Color.White, new Rectangle(942, 780, 310, 80)), eventGameModeLan.OnMainMenu, new Text(montserratBold, "Main Menu", Color.Black, 1f, true)),
                    });
                }
                finished = true;
            }
        }

        private void AddElement()
        {
            if (_tempElements.Count != 0)
            {
                _elements.AddRange(_tempElements);
                _tempElements.Clear();
            }
        }

        private void DisposeElement()
        {
            if (_elements.Count != 0)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    IBaseDisposable baseDisposable = _elements[i];
                    if (baseDisposable.isDisposed)
                    {
                        if (_elements[i] is ICollisionActor actor)
                        {
                            _collisionComponent.Remove(actor);
                        }

                        _elements.RemoveAt(i);
                    }
                }
            }
        }
    }
}