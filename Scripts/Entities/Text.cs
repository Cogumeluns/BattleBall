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
        private Texture2D texture2D;
        private Rectangle rect;
        private Color colorBackground;
        public SpriteFont spriteFont;
        public string text;
        public Vector2 position = Vector2.Zero;
        private Color color;
        public float scale;
        public bool isDisposed { get; set; }

        public bool isVisible;

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

        public Text(SpriteFont spriteFont, string text, Color color, float scale, bool isVisible, Vector2 position, Texture2D textureBackground, Color colorBackground)
        : this(spriteFont, text, color, scale, isVisible, position)
        {
            this.colorBackground = colorBackground;
            texture2D = textureBackground;
            AdjustPosition();
        }

        void AdjustPosition()
        {
            Vector2 textSize = spriteFont.MeasureString(text) * scale;
            rect = new((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture2D != null)
                spriteBatch.Draw(texture2D, rect, colorBackground);

            spriteBatch.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            // Não tem nada para atualizar.
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}