using System;
using System.Collections.Generic;
using BattleBall;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

public class LocalMode : GameScreen
{
    public new GameMain Game => (GameMain)base.Game;
    public LocalMode(GameMain game) : base(game) { }
    List<IUpdateDrawable> _elements = new();
    EventLocalMode eventLocalMode;

    public override void LoadContent()
    {
        eventLocalMode = new EventLocalMode(this);

        SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");
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

            new Button(new Image(button, Color.White, new Rectangle(1083, 900, 310, 80)), eventLocalMode.OnStartGameMode, new Text(montserratBold, "START", Color.Black, 1f, true))
        });
        base.LoadContent();
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Update(GameTime gameTime)
    {
        _elements.ForEach(x => x.Update(gameTime));
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Black);

        Game.SpriteBatch.Begin();
        _elements.ForEach(x => x.Draw(Game.SpriteBatch));
        Game.SpriteBatch.End();
    }
}