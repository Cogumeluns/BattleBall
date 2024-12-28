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
        public Vector2 position;
        private Color color;
        public float scale;
        public bool isDisposed { get; set; }

        public Text(SpriteFont spriteFont, string text, Vector2 position, Color color, float scale)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.color = color;
            this.scale = scale;
        }

        public Text(SpriteFont spriteFont, string text, Vector2 position, Color color, float scale, Texture2D textureBackground, Color colorBackground)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.color = color;
            this.scale = scale;
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