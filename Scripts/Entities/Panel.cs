using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleBall.Scripts.Entities
{
    public class Panel : IUpdateDrawable
    {
        public bool isDisposed { get; set; }

        private List<IUpdateDrawable> _elements = new();

        private Texture2D texture2D;
        private Rectangle rect;
        private Color colorBackground;

        public Panel(Texture2D texture2D, Rectangle rect, Color color)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.colorBackground = color;
        }

        public void AddTexts(List<Text> texts, float spacing)
        {
            float currentY = rect.Y;

            foreach (var item in texts)
            {
                Vector2 textMeasure = item.spriteFont.MeasureString(item.text) * item.scale;

                item.position += new Vector2(rect.X + 30, currentY + 10);

                currentY += textMeasure.Y + spacing;

                _elements.Add(item);
            }
        }

        public void Dispose()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, rect, colorBackground);
            _elements.ForEach(x => x.Draw(spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            // Não faz nada.
        }
    }
}