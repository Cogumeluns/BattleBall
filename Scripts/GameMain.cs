using BattleBall.Scripts.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace BattleBall;

public class GameMain : Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch SpriteBatch;
    public Size GameBound = new(1440, 1024);
    public readonly GameSceneManager gameSceneManager;
    public Mix mix;

    public GameMain()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBound.Width;
        _graphics.PreferredBackBufferHeight = GameBound.Height;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        var _screenManager = new ScreenManager();
        gameSceneManager = new GameSceneManager(this, _screenManager);
        Components.Add(_screenManager);
        mix = new Mix(this);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        gameSceneManager.LoadScene(Scene.MAIN_MENU);


        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
