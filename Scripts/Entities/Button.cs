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
        public bool isDisposed => throw new NotImplementedException();
        private MouseState mouseState = Mouse.GetState();
        public event EventHandler OnClick;
        private bool isClicked;
        private float alpha;


        public Button(Texture2D t, Vector2 p, Size s, EventHandler OnClick)
        {
            _texture = t;
            _rect = new((int)p.X, (int)p.Y, s.Width, s.Height);
            this.OnClick = OnClick;

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
            HandlerMouseKeyPress();
            HandlerOpacity();
        }

        private void Click()
        {
            isClicked = true;
            OnClick?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rect, Color.White * alpha);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}