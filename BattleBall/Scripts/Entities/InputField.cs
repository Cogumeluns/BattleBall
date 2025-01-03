using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleBall.Scripts.Interfaces;

namespace BattleBall.Scripts.Entities
{
    public class InputField : IUpdateDrawable
    {
        // IUpdateDrawable
        public bool isDisposed { get; set; }
        // IUpdateDrawable
        public bool isVisible { get; set; } = true;
        public const string PLACEHOLD = "IP...";
        private const int SPACINGLEFT = -150;

        private Button _button;
        public Text text;
        private Text _placeHold;
        private event EventHandler _OnEndEdit;

        // Public properties
        private string Text
        {
            get => text.text;
            set => text.text = value;
        }
        public bool IsActive { get; set; } = false;
        private KeyboardState previousKeyboardState = Keyboard.GetState();

        // Constructor
        public InputField(Button button, Text text, EventHandler OnEndEdit)
        {
            _button = button;
            _button._OnClick += OnActive;
            this._OnEndEdit = OnEndEdit;
            this.text = text;
            _placeHold = new Text(text.spriteFont, PLACEHOLD, Color.Gray, 1f, true);

            _placeHold.AdjustCenterPosition(_button.rect, SPACINGLEFT, 0);
        }

        private void AdjustTextPosition()
        {
            Vector2 textMeasure = text.GetMeasureText();
            text.position = new Vector2(_button.rect.X + 45, _button.rect.Y + (_button.rect.Height - textMeasure.Y) / 2);
        }

        // Events
        private void OnActive(object sender, EventArgs e)
        {
            IsActive = true;
        }

        private void OnEndEdit()
        {
            // Event to handle end of input (e.g., send to server)
            _OnEndEdit?.Invoke(this, EventArgs.Empty);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            _button.Update(gameTime);

            if (!IsActive) return;

            KeyboardState keyboardState = Keyboard.GetState();

            foreach (var key in keyboardState.GetPressedKeys())
            {
                if (!previousKeyboardState.IsKeyDown(key))
                {
                    HandleKeyInput(key);
                }
            }

            AdjustTextPosition();
            previousKeyboardState = keyboardState;
        }

        private void HandleKeyInput(Keys key)
        {
            if (key == Keys.Back && Text.Length > 0)
            {
                Text = Text[..^1];
            }
            else if (key == Keys.Enter)
            {
                IsActive = false;
                OnEndEdit();
            }
            else if (text.text.Length >= 20)
            {
                return;
            }
            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                AddCharacter(key.ToString()[1]);
            }
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                AddCharacter((char)('0' + (key - Keys.NumPad0)));
            }
            else if (key == Keys.OemPeriod || key == Keys.Decimal)
            {
                AddCharacter('.');
            } else
            {
                AddCharacter(key.ToString()[0]);
            }
        }

        private void AddCharacter(char c)
        {
            if (ValidateIP(Text + c))
            {
                Text += c;
            }
        }

        private bool ValidateIP(string ip)
        {
            string[] parts = ip.Split('.');
            if (parts.Length > 4) return false;

            foreach (var part in parts)
            {
                if (part.Length > 3) return false;
                if (part.Length > 0 && int.TryParse(part, out int value) && value > 255) return false;
            }

            return true;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            _button.Draw(spriteBatch);

            if (string.IsNullOrEmpty(Text))
            {
                DrawPlaceHold(spriteBatch);
            }
            else
            {
                AdjustTextPosition();
                text.Draw(spriteBatch);
            }

        }

        private void DrawPlaceHold(SpriteBatch spriteBatch)
        {
            _placeHold.Draw(spriteBatch);
        }

        // Dispose
        public void Dispose() { }
    }
}
