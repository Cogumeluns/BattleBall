using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Enum;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;

namespace BattleBall.Scripts.Scene
{
    public class GameMode : GameScreen
    {
        public new GameMain Game => (GameMain)base.Game;
        public GameMode(GameMain game) : base(game) { }

        public bool isDisposed { get; private set; }

        private CollisionComponent _collisionComponent;

        List<IUpdateDrawable> _elements = new();
        List<IUpdateDrawable> _tempElements = new();

        EventGameMode eventGameMode;

        Player player1;
        Player player2;
        Ball ball;
        Field field;
        ControllerBallLight controllerBallLight;
        Timer timer;

        public override void LoadContent()
        {
            float heigth = Game._graphics.PreferredBackBufferHeight;
            float width = Game._graphics.PreferredBackBufferWidth;

            _collisionComponent = new CollisionComponent(new Rectangle(0, 0, (int)width, (int)heigth));
            eventGameMode = new(this);

            player1 = new(new(new(width / 4, heigth / 2), 30), new(81, 125, 201), true);
            player1.SetKeys(new Dictionary<PlayerKeys, Func<KeyboardState, bool>>() {
                    { PlayerKeys.Up, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.W) },
                    { PlayerKeys.Down, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.S) },
                    { PlayerKeys.Left, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.A) },
                    { PlayerKeys.Right, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.D) },
                    { PlayerKeys.Dash, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Space) }
                });

            player2 = new(new(new(width - (width / 4), heigth / 2), 30), new(255, 106, 106), false);
            player2.SetKeys(new Dictionary<PlayerKeys, Func<KeyboardState, bool>>() {
                { PlayerKeys.Up, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Up) },
                { PlayerKeys.Down, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Down) },
                { PlayerKeys.Left, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Left) },
                { PlayerKeys.Right, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.Right) },
                { PlayerKeys.Dash, (KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.NumPad0) }
            });

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

            _collisionComponent.Update(gameTime);

            _elements.ForEach(x =>
            {
                if (!x.isVisible) return;
                x.Update(gameTime);
            });
        }

        private void FinishedMode()
        {
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

                _tempElements.AddRange(new List<IUpdateDrawable>()
                {
                    new Text(modak, text, color, 1f, true, new((int)((width - modak.MeasureString(text).X)/2), 100)),
                    new Button(new Image(button, Color.White, new Rectangle(187, 780, 310, 80)), eventGameMode.OnPlayAgain, new Text(montserratBold, "Play Again", Color.Black, 1f, true)),
                    new Button(new Image(button, Color.White, new Rectangle(942, 780, 310, 80)), eventGameMode.OnMainMenu, new Text(montserratBold, "Main Menu", Color.Black, 1f, true)),
                });
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