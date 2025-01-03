using System;
using System.Security.Cryptography.X509Certificates;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BattleBall.Scripts.Entities
{
    public class Button : IUpdateDrawable
    {
        // IUpdateDrawable
        public bool isDisposed { get; private set; }
        // IUpdateDrawable
        public bool isVisible { get; set; } = true;

        private Image _image;
        public Rectangle rect { get => _image.rect; }
        private MouseState _mouseState = Mouse.GetState();
        public event EventHandler _OnClick;
        private Text _text;
        private bool _isClicked;

        public Button(Image image)
        {
            _image = image;
        }

        public Button(Image image, EventHandler OnClick)
        : this(image)
        {
            this._OnClick = OnClick;
        }

        public Button(Image image, EventHandler OnClick, Text text)
        : this(image, OnClick)
        {
            _text = text;
            _text.AdjustCenterPosition(image.rect);
        }

        Vector2 GetPositionMouse() => new(_mouseState.X, _mouseState.Y);

        bool Intersect(Vector2 pointer, Rectangle rect)
        {
            return rect.Intersects(new((int)pointer.X, (int)pointer.Y, 1, 1));
        }

        void HandlerMouseKeyPress()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (Intersect(GetPositionMouse(), rect))
                    Click();
            }
        }

        void HandlerOpacity()
        {
            if (Intersect(GetPositionMouse(), rect) && !_isClicked)
            {
                _image.alpha = 0.5f;
            }
            else
            {
                _isClicked = false;
                _image.alpha = 1f;
            }
        }

        public void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            HandlerOpacity();
            HandlerMouseKeyPress();
        }

        private void Click()
        {
            _isClicked = true;
            _OnClick?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _image.Draw(spriteBatch);
            if (_text != null)
                _text.Draw(spriteBatch);
        }

        // IUpdateDrawable
        public void Dispose() { }
    }
}