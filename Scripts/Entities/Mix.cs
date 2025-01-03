using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace BattleBall.Scripts.Entities
{
    public class Mix
    {
        public Song menuMix;
        public Song battleMix;

        public global::Scene current;

        public global::Scene scene
        {
            set
            {
                if (value == global::Scene.GAME_MODE)
                {
                    if (MediaPlayer.Queue.ActiveSong == menuMix)
                    {
                        PlaySong(battleMix);
                    }
                }
                else
                {
                    if (MediaPlayer.Queue.ActiveSong == battleMix || MediaPlayer.Queue.ActiveSong == null)
                    {
                        PlaySong(menuMix);
                    }
                }
            }
        }

        public Mix(GameMain game)
        {
            menuMix = game.Content.Load<Song>("mix/menu");
            battleMix = game.Content.Load<Song>("mix/battle");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }

        private void PlaySong(Song song)
        {
            MediaPlayer.Play(song);
        }
    }
}