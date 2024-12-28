using BattleBall;
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
        switch (scene)
        {
            case Scene.MAIN_MENU:
                _screenManager.LoadScreen(new MainMenu(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
            case Scene.SCENE_2:
                _screenManager.LoadScreen(new Scene2(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
                break;
        }
    }
}

public enum Scene
{
    MAIN_MENU, SCENE_2
}