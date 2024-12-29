using System;
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
    public new GameMain Game => (GameMain)base.Game;
    public LanMode(GameMain game, bool isMaster) : base(game)
    {
        this.isMaster = isMaster;
    }

    List<IUpdateDrawable> _elements = new();
    EventLanMode eventLanMode;
    List<Text> connecting = new();
    public bool isMaster;
    public bool tempConnecting = false; // um exemplo de como mudar a cena.

    public override void LoadContent()
    {
        eventLanMode = new(this);

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
        });

        if (isMaster)
            OnLoadContentMaster(button, montserratRegular, montserratBold);
        else
            OnLoadContentClient(button, montserratRegular, montserratBold);
        base.LoadContent();
    }


    private void OnLoadContentClient(Texture2D button, SpriteFont montserratRegular, SpriteFont montserratBold)
    {
        _elements.Add(new InputField(button, new(500, 773, 440, 78), montserratRegular));

        int xFailure = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Failure").X) / 2);
        int xConnect = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Connected").X) / 2);

        connecting.AddRange(new List<Text>()
        {
            new Text(montserratBold, "Failure", Color.Red, 1f, true, new(xFailure, 869)),
            new Text(montserratBold, "Connected", Color.Green, 1f, true, new(xConnect, 869)),
        });

        _elements.AddRange(connecting);
    }

    private void OnLoadContentMaster(Texture2D button, SpriteFont montserratRegular, SpriteFont montserratBold)
    {
        _elements.Add(new Button(button, new(1083, 900), new(310, 80), new Text(montserratBold, "START", Color.Black, 1f, true), eventLanMode.OnStartGameMode));

        int xWainting = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Waiting...").X) / 2);
        int xConnect = (int)((Game._graphics.PreferredBackBufferWidth - montserratBold.MeasureString("Connected").X) / 2);

        connecting.AddRange(new List<Text>()
        {
            new Text(montserratBold, "Waiting...", new(95,95,95), 1f, true, new(xWainting, 869)),
            new Text(montserratBold, "Connected", Color.Green, 1f, true, new(xConnect, 869)),
        });

        _elements.AddRange(connecting);
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Update(GameTime gameTime)
    {
        ChangeTextConnectingActive();

        _elements.ForEach(x => x.Update(gameTime));
    }

    private void ChangeTextConnectingActive()
    {
        if (tempConnecting)
        {
            connecting[1].isVisible = true;
            connecting[0].isVisible = false;
        }
        else
        {
            connecting[0].isVisible = true;
            connecting[1].isVisible = false;
        }
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
}