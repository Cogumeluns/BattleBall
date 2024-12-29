using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleBall.Scripts.Entities
{
    public class Text : IUpdateDrawable
    {
        private Image _image;
        public SpriteFont spriteFont;
        public string text;
        public Vector2 position = Vector2.Zero;
        private Color color;
        public float scale;
        public bool isDisposed { get; set; }

        public bool isVisible { get; set; } = true;

        public Text(SpriteFont spriteFont, string text, Color color, float scale, bool isVisible)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.color = color;
            this.scale = scale;
            this.isVisible = isVisible;
        }

        public Text(SpriteFont spriteFont, string text, Color color, float scale, bool isVisible, Vector2 position)
        : this(spriteFont, text, color, scale, isVisible)
        {
            this.position = position;
        }

        public Text(SpriteFont spriteFont, string text, Color color, float scale, bool isVisible, Vector2 position, Image image)
        : this(spriteFont, text, color, scale, isVisible, position)
        {
            this._image = image;
            AdjustPosition();
        }

        public Vector2 GetMeasureText() => spriteFont.MeasureString(text) * scale;

        public void AdjustCenterPosition(Rectangle rect)
        {
            Vector2 textMeasure = GetMeasureText();
            Vector2 textP = new(rect.X, rect.Y);
            textP.X += (rect.Width - textMeasure.X) / 2;
            textP.Y += (rect.Height - textMeasure.Y) / 2;
            position = textP;
        }

        public void AdjustCenterPosition(Rectangle rect, int someX, int someY)
        {
            rect.X += someX;
            rect.Y += someY;
            AdjustCenterPosition(rect);
        }

        void AdjustPosition()
        {
            Vector2 textSize = GetMeasureText();
            _image.rect = new((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_image != null)
                _image.Draw(spriteBatch);

            spriteBatch.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime) { }

        public void Dispose() { }
    }
}