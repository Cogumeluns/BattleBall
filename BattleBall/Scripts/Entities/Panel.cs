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
        public bool isVisible { get; set; } = true;
        public bool isDisposed { get; set; }

        private List<IUpdateDrawable> _elements = new();

        private Image _image;

        public Panel(Image image)
        {
            _image = image;
        }

        public void AddTexts(List<Text> texts, float spacing)
        {
            float currentY = _image.rect.Y;

            foreach (var item in texts)
            {
                Vector2 textMeasure = item.spriteFont.MeasureString(item.text) * item.scale;

                item.position += new Vector2(_image.rect.X + 54, currentY + 33);

                currentY += textMeasure.Y + spacing;

                _elements.Add(item);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _image.Draw(spriteBatch);
            _elements.ForEach(x => x.Draw(spriteBatch));
        }

        public void Dispose() { }

        public void Update(GameTime gameTime) { }
    }
}