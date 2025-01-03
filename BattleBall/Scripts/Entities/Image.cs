using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleBall.Scripts.Entities
{
    public class Image : IUpdateDrawable
    {
        // IUpdateDrawable
        public bool isDisposed { get; set; }
        // IUpdateDrawable
        public bool isVisible { get; set; } = true;
        private Texture2D texture2D;
        public Rectangle rect;
        private Color color;
        public float alpha;

        public Image(Texture2D texture2D, Color color, Rectangle rect, float alpha = 1f)
        : this(texture2D, color, alpha)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.color = color;
            this.alpha = alpha;
        }

        public Image(Texture2D texture2D, Color color, float alpha = 1f)
        {
            this.texture2D = texture2D;
            this.color = color;
            this.alpha = alpha;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, rect, color * alpha);
        }

        public void Update(GameTime gameTime) { }

        public void Dispose() { }
    }
}