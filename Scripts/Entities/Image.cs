using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleBall.Scripts.Entities
{
    public class Image : IUpdateDrawable
    {
        public bool isDisposed { get; set; }
        private Texture2D texture2D;
        private Rectangle rect;
        private Color color;

        public Image(Texture2D texture2D, Rectangle rect, Color color)
        {
            this.texture2D = texture2D;
            this.rect = rect;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, rect, color);
        }

        public void Update(GameTime gameTime) { }

        public void Dispose() { }
    }
}