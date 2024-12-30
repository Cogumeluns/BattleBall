using BattleBall;
using BattleBall.Scripts.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

public class GameSceneManager
{
    private readonly GameMain _game;
    public readonly ScreenManager _screenManager;

    public GameSceneManager(GameMain game, ScreenManager screenManager)
    {
        _game = game;
        _screenManager = screenManager;
    }

    public void LoadScene(Scene scene)
    {
        _game.mix.scene = scene;
        switch (scene)
        {
            case Scene.MAIN_MENU:
                _screenManager.LoadScreen(new MainMenu(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.LOCAL_MODE:
                _screenManager.LoadScreen(new LocalMode(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.LAN_MODE:
                _screenManager.LoadScreen(new LanMode(_game, true), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.CLIENT_MODE:
                _screenManager.LoadScreen(new LanMode(_game, false), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.GAME_MODE:
                _screenManager.LoadScreen(new GameMode(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.ABOUT:
                _screenManager.LoadScreen(new About(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
        }
    }
}

public enum Scene
{
    MAIN_MENU, LAN_MODE, CLIENT_MODE, LOCAL_MODE, GAME_MODE, ABOUT
}