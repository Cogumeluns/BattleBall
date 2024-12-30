using BattleBall;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

public class Scene1 : GameScreen
{
    private new GameMain Game => (GameMain) base.Game;

    public Connection Connection { get; private set; }
    
    private Texture2D _logo;
    private SpriteFont _font;
    private Vector2 _position = new Vector2(50,50);
    public Scene1(GameMain game) : base(game) { 
        Connection = new Connection();
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _font = Game.Content.Load<SpriteFont>("fonts/arial");
        _logo = Game.Content.Load<Texture2D>("textures/texture");
    }

    public override void Update(GameTime gameTime)
    {
        _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Up))
        {
            Game.gameSceneManager.LoadScene(Scene.SCENE_2);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(new Color(16, 139, 204));
        Game.SpriteBatch.Begin();
        Game.SpriteBatch.DrawString(_font, nameof(Scene1), new Vector2(10,10), Color.White);
        Game.SpriteBatch.Draw(_logo, _position, Color.White);
        Game.SpriteBatch.End();
    }
}