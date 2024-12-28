using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace BattleBall;

public class GameMain : Game
{
    private GraphicsDeviceManager _graphics;
    
    private readonly ScreenManager _screenManager;

    public SpriteBatch SpriteBatch;

    public GameMain()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _screenManager = new ScreenManager();
        Components.Add(_screenManager);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        LoadScreen1();
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
        if (keyboardState.IsKeyDown(Keys.Down))
        {
            LoadScreen1();
        }
        else if (keyboardState.IsKeyDown(Keys.Up))
        {
            LoadScreen2();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private void LoadScreen1()
    {
        _screenManager.LoadScreen(new Screen1(this), new FadeTransition(GraphicsDevice, Color.Black));
    }

    private void LoadScreen2()
    {
        _screenManager.LoadScreen(new MyScreen2(this), new FadeTransition(GraphicsDevice, Color.Black));
    }

}
