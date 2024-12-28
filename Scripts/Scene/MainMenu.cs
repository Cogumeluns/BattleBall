using System;
using System.Collections.Generic;
using BattleBall;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

public class MainMenu : GameScreen
{
    private new GameMain Game => (GameMain)base.Game;
    public MainMenu(GameMain game) : base(game) { }

    List<IUpdateDrawable> _elements = new();
    public override void LoadContent()
    {
        _elements.AddRange(new List<IUpdateDrawable>()
            {
                new Button(Content.Load<Texture2D>("textures/button"), new(100, 300), new(365, 80),
                            new Text(Content.Load<SpriteFont>("fonts/montserratbold"), "Click", Color.Black, 2f), MyClickHandler),
                new Text(Content.Load<SpriteFont>("fonts/modak"), "Title", Vector2.Zero, Color.White, 1f, Content.Load<Texture2D>("textures/button"), Color.Green)
            }
        );
        base.LoadContent();
    }

    public void MyClickHandler(object sender, EventArgs e)
    {
        Console.WriteLine("O evento OnClick foi disparado!");
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