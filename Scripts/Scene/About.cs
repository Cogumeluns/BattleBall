using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace BattleBall.Scripts.Scene
{
    public class About : GameScreen
    {
        public new GameMain Game => (GameMain)base.Game;
        public About(GameMain game) : base(game) { }

        public bool isDisposed { get; private set; }

        List<IUpdateDrawable> _elements = new();

        EventAbout eventAbout;

        public override void LoadContent()
        {
            eventAbout = new(this);

            SpriteFont modak = Content.Load<SpriteFont>("fonts/modak");
            SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");
            SpriteFont montserratItalic = Content.Load<SpriteFont>("fonts/montserratitalic");

            Texture2D button = Content.Load<Texture2D>("textures/button");
            Texture2D panelPNG = Content.Load<Texture2D>("textures/panel");
            Texture2D about = Content.Load<Texture2D>("textures/about");
            Texture2D logo = Content.Load<Texture2D>("textures/project-logo");

            var panel = new Panel(panelPNG, new(163, 40, 1113, 300), Color.White);

            panel.AddTexts(new List<Text>()
            {
                new Text(montserratItalic, "Project: Ping Pong Battle", Color.White, 0.8f, true),
                new Text(montserratItalic, "By:", Color.White, 0.8f, true),
                new Text(montserratItalic, "    George P.", Color.White, 0.8f, true),
                new Text(montserratItalic, "    Otaviano M.", Color.White, 0.8f, true),
                new Text(montserratItalic, "Cogumeluns Group", Color.White, 0.8f, true, new(375, 62)),
            }, 1f);

            _elements.AddRange(new List<IUpdateDrawable>()
            {
                new Image(logo, new(205, 903, 92, 92), Color.White),
                new Image(about, new(71, 903, 92, 92), Color.White),
                new Button(button, new(1105, 916), new(270, 60), new(montserratBold, "Return", Color.Black, 1f, true), eventAbout.OnReturn),
                panel,

            });

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
            _elements.ForEach(x => x.Draw(Game.SpriteBatch));
            Game.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _elements.ForEach(x => x.Update(gameTime));
        }
    }
}