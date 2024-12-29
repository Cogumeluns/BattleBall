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
            new Button(button, new(537, 500), new(365, 80), new Text(montserratBold, "Local Mode", Color.Black, 1f, true), eventMainMenu.OnLocalMode),
            new Button(button, new(537, 605), new(365, 80), new Text(montserratBold, "Lan Mode", Color.Black, 1f, true), eventMainMenu.OnLanMode),
            new Button(button, new(537, 710), new(365, 80), new Text(montserratBold, "Client Mode", Color.Black, 1f, true), eventMainMenu.OnClientMode),
            new Button(button, new(537, 815), new(365, 80), new Text(montserratBold, "Quit", Color.Black, 1f, true), eventMainMenu.OnQuit),
            new Button(about, new(1340, 930), new(68, 68), new Text(montserratBold, "I", Color.Black, 1f, true), eventMainMenu.OnAbout),
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