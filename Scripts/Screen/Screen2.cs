using BattleBall;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

public class MyScreen2 : GameScreen
{
    private new GameMain Game => (GameMain)base.Game;
    private Texture2D _logo;
    private SpriteFont _font;
    private Vector2 _position = new Vector2(50, 50);
    
    public MyScreen2(GameMain game) : base(game) { }

    public override void LoadContent()
    {
        base.LoadContent();
        // _font = Game.Content.Load<SpriteFont>("font");
        // _logo = Game.Content.Load<Texture2D>("logo-mg");
    }

    public override void Update(GameTime gameTime)
    {
        _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.White);
        Game.SpriteBatch.Begin();
        // Game.SpriteBatch.DrawString(_font, nameof(MyScreen2), new Vector2(10, 10), Color.Orange);
        // Game.SpriteBatch.Draw(_logo, _position, Color.White);
        Game.SpriteBatch.End();
    }
}