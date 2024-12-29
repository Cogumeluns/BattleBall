using System;
using System.Collections.Generic;
using BattleBall;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

public class MainMenu : GameScreen
{
    public new GameMain Game => (GameMain)base.Game;
    public MainMenu(GameMain game) : base(game) { }
    List<IUpdateDrawable> _elements = new();
    EventMainMenu eventMainMenu;
    public override void LoadContent()
    {
        eventMainMenu = new EventMainMenu(this);

        SpriteFont modak = Content.Load<SpriteFont>("fonts/modak");
        SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");

        Texture2D button = Content.Load<Texture2D>("textures/button");
        Texture2D about = Content.Load<Texture2D>("textures/about");

        _elements.AddRange(new List<IUpdateDrawable>()
        {
            new Text(modak, "PING PONG BATTLE", Color.White, 0.8f, true, new(155, 80)),
            new Button(new Image(button, Color.White, new Rectangle(537, 500, 365, 80)), eventMainMenu.OnLocalMode, new Text(montserratBold, "Local Mode", Color.Black, 1f, true)),
            new Button(new Image(button, Color.White, new Rectangle(537, 605, 365, 80)), eventMainMenu.OnLanMode, new Text(montserratBold, "Lan Mode", Color.Black, 1f, true)),
            new Button(new Image(button, Color.White, new Rectangle(537, 710, 365, 80)), eventMainMenu.OnClientMode, new Text(montserratBold, "Client Mode", Color.Black, 1f, true)),
            new Button(new Image(button, Color.White, new Rectangle(537, 815, 365, 80)), eventMainMenu.OnQuit, new Text(montserratBold, "Quit", Color.Black, 1f, true)),
            new Button(new Image(about, Color.White, new Rectangle(1340, 930, 68, 68)), eventMainMenu.OnAbout, new Text(montserratBold, "I", Color.Black, 1f, true)),
        }
        );
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