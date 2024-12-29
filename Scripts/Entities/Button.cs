using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BattleBall.Scripts.Entities
{
    public class Button : IUpdateDrawable
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _rect;
        private Color _shade = Color.White;
        public bool isDisposed { get; private set; }
        private MouseState mouseState = Mouse.GetState();
        public event EventHandler OnClick;
        private bool isClicked;
        private float alpha;
        private Text text;

        public bool isVisible { get; set; } = true;


        public Button(Texture2D t, Vector2 p, Size s, Text text, EventHandler OnClick)
        {
            _texture = t;
            _rect = new((int)p.X, (int)p.Y, s.Width, s.Height);
            this.OnClick = OnClick;
            this.text = text;
            AdjustPosition(p, s);
        }

        public Button(Texture2D t, Vector2 p, Size s, EventHandler OnClick)
        {
            _texture = t;
            _rect = new((int)p.X, (int)p.Y, s.Width, s.Height);
            this.OnClick = OnClick;
        }

        public void AdjustPosition(Vector2 p, Size s)
        {
            Vector2 textMeasure = text.spriteFont.MeasureString(text.text) * text.scale;
            Vector2 textP = p;
            textP.X += (s.Width - textMeasure.X) / 2;
            textP.Y += (s.Height - textMeasure.Y) / 2;

            text.position = textP;
        }

        Vector2 GetPositionMouse() => new(mouseState.X, mouseState.Y);

        bool Intersect(Vector2 pointer, Rectangle rect)
        {
            return rect.Intersects(new((int)pointer.X, (int)pointer.Y, 1, 1));
        }

        void HandlerMouseKeyPress()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Intersect(GetPositionMouse(), _rect))
                    Click();
            }
        }

        void HandlerOpacity()
        {
            if (Intersect(GetPositionMouse(), _rect) && !isClicked)
            {
                alpha = 0.5f;
            }
            else
            {
                isClicked = false;
                alpha = 1f;
            }
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            HandlerOpacity();
            HandlerMouseKeyPress();
        }

        private void Click()
        {
            isClicked = true;
            OnClick?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rect, Color.White * alpha);
            if (text != null)
                text.Draw(spriteBatch);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}