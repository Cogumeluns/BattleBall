using System.Collections.Generic;
using BattleBall;
using BattleBall.Scripts.Entities;
using BattleBall.Scripts.Events;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

public class LanMode : GameScreen
{
    private new GameMain Game => (GameMain)base.Game;
    public LanMode(GameMain game) : base(game) { }

    List<IUpdateDrawable> _elements = new();
    EventLanMode eventLanMode = new();

    public override void LoadContent()
    {
        SpriteFont montserratBold = Content.Load<SpriteFont>("fonts/montserratbold");
        SpriteFont montserratRegular = Content.Load<SpriteFont>("fonts/montserratregular");

        Texture2D button = Content.Load<Texture2D>("textures/button");
        Texture2D cicle = Content.Load<Texture2D>("textures/about");
        Texture2D rectangle = Content.Load<Texture2D>("textures/rectangle");

        _elements.AddRange(new List<IUpdateDrawable>()
        {
            new Image(cicle, new(310, 80, 160, 160), new(81, 125, 201)),
            new Text(montserratBold, " W ", Color.Black, 1f, true, new(365, 300), rectangle, Color.White),
            new Text(montserratBold, " S ", Color.Black, 1f, true, new(365, 370), rectangle, Color.White),
            new Text(montserratBold, " A", Color.Black, 1f, true, new(365, 440), rectangle, Color.White),
            new Text(montserratBold, " D ", Color.Black, 1f, true, new(365, 510), rectangle, Color.White),
            new Text(montserratBold, " Space ", Color.Black, 1f, true, new(365, 580), rectangle, Color.White),

            new Text(montserratBold, " Up ", Color.Black, 1f, true, new(700, 300), rectangle, Color.White),
            new Text(montserratBold, " Down ", Color.Black, 1f, true, new(700, 370), rectangle, Color.White),
            new Text(montserratBold, " Left", Color.Black, 1f, true, new(700, 440), rectangle, Color.White),
            new Text(montserratBold, " Right ", Color.Black, 1f, true, new(700, 510), rectangle, Color.White),
            new Text(montserratBold, " Dash ", Color.Black, 1f, true, new(700, 580), rectangle, Color.White),

            new Image(cicle, new(1003, 80, 160, 160), new(255, 106, 106)),
            new Text(montserratBold, " Up ", Color.Black, 1f, true, new(1058, 300), rectangle, Color.White),
            new Text(montserratBold, " Down ", Color.Black, 1f, true, new(1058, 370), rectangle, Color.White),
            new Text(montserratBold, " Left ", Color.Black, 1f, true, new(1058, 440), rectangle, Color.White),
            new Text(montserratBold, " Right ", Color.Black, 1f, true, new(1058, 510), rectangle, Color.White),
            new Text(montserratBold, " 0 ", Color.Black, 1f, true, new(1058, 580), rectangle, Color.White),

            new InputField(button, new(500, 773, 440, 78), montserratRegular),

            new Button(button, new(1083, 900), new(310, 80), new Text(montserratBold, "START", Color.Black, 1f, true), null)
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