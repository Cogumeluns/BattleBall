using System.Collections.Generic;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BattleBall;

public class GameMain : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Point GameBounds = new Point(1280, 720);

    private CollisionComponent _collisionComponent;

    List<IUpdateDrawable> updateDrawables = new();

    _Player player1;
    _Player player2;

    _Field field;

    public GameMain()
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
        player1.SetKeys(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q);

        player2 = new(new(new(GameBounds.X - (GameBounds.X / 4) - 500, GameBounds.Y / 2), 30), Color.Red);
        player2.SetKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.E);

        field = new(new(new(50, 50), new(GameBounds.X - 100, GameBounds.Y - 100)), Color.White);

        _collisionComponent.Insert(player1);
        _collisionComponent.Insert(player2);
        _collisionComponent.Insert(field);
        updateDrawables.Add(player1);
        updateDrawables.Add(player2);
        updateDrawables.Add(field);

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

        updateDrawables.ForEach(x => x.Update(gameTime));

        _collisionComponent.Update(gameTime);

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
