using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleBall.Scripts.Interfaces;

namespace BattleBall.Scripts.Entities
{
    public class InputField : IUpdateDrawable
    {
        // Public properties
        public Texture2D Texture { get; set; }
        public string Text { get; private set; } = "";
        public string PlaceHold { get; private set; } = "IP...";
        public Rectangle Bounds { get; }
        public SpriteFont Font { get; }
        public Color TextColor { get; set; } = Color.Black;
        public bool IsActive { get; set; } = false;
        public bool IsDisposed { get; set; }

        // Private fields
        private float scale = 0.8f;
        private Vector2 TextPosition { get; set; }
        private Vector2 PlaceHoldPosition { get; set; }

        public bool isDisposed => throw new NotImplementedException();

        private KeyboardState previousKeyboardState = Keyboard.GetState();
        private readonly Button button;

        // Constructor
        public InputField(Texture2D texture, Rectangle bounds, SpriteFont font)
        {
            Texture = texture;
            Bounds = bounds;
            Font = font;

            button = new Button(texture, new(bounds.X, bounds.Y), new(bounds.Width, bounds.Height), OnActive);
            AdjustPlaceHoldPosition();
        }

        // Adjust positions
        private void AdjustTextPosition()
        {
            Vector2 textMeasure = Font.MeasureString(Text) * scale;
            TextPosition = new Vector2(Bounds.X + 45, Bounds.Y + (Bounds.Height - textMeasure.Y) / 2);
        }

        private void AdjustPlaceHoldPosition()
        {
            Vector2 textMeasure = Font.MeasureString(PlaceHold) * scale;
            PlaceHoldPosition = new Vector2(Bounds.X + 45, Bounds.Y + (Bounds.Height - textMeasure.Y) / 2);
        }

        // Events
        private void OnActive(object sender, EventArgs e)
        {
            IsActive = true;
        }

        private void OnEndEdit()
        {
            // Event to handle end of input (e.g., send to server)
        }

        // Update
        public void Update(GameTime gameTime)
        {
            button.Update(gameTime);

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
            }
            else if (key == Keys.Enter)
            {
                IsActive = false;
                OnEndEdit();
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
            button.Draw(spriteBatch);

            if (string.IsNullOrEmpty(Text))
            {
                DrawPlaceHold(spriteBatch);
            }

            spriteBatch.DrawString(Font, Text, TextPosition, TextColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private void DrawPlaceHold(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, PlaceHold, PlaceHoldPosition, Color.Gray, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        // Dispose
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
