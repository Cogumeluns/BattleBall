using System;
using System.Collections.Generic;
using BattleBall.Scripts.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleBall.Scripts.Entities
{
    public class Timer : IUpdateDrawable
    {
        // IUpdateDrawable
        public bool isVisible { get; set; } = true;
        // IUpdateDrawable
        public bool isDisposed { get; set; } = false;
        private Text _text;
        private float time;
        public SpriteFont Font { get; set; }
        public bool IsFinished { get; private set; } = false;

        public List<Player> Players { get; set; } = new();

        public Timer(Text text, Player p1, Player p2, float startTime = 300f)
        {
            _text = text;
            time = startTime;
            Players.Add(p1);
            Players.Add(p2);
            _text.text = FormatTime(time);
        }

        public void Finished()
        {
            if (Players[0].Lives < Players[1].Lives)
            {
                Players[0].Damage(Players[0].Lives);
            }
            else if (Players[0].Lives > Players[1].Lives)
            {
                Players[1].Damage(Players[1].Lives);
            }
            else
            {
                Players[0].Damage(Players[0].Lives);
                Players[1].Damage(Players[1].Lives);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (time <= 0)
            {
                time = 0;
                IsFinished = true;
                Finished();
            }

            _text.text = FormatTime(time);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _text.Draw(spriteBatch);
        }

        private string FormatTime(float time)
        {
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}
