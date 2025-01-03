using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Scene;

namespace BattleBall.Scripts.Events
{
    public class EventAbout
    {
        public About About;

        public EventAbout(About About)
        {
            this.About = About;
        }

        public void OnReturn(object sender, EventArgs e)
        {
            About.Game.gameSceneManager.LoadScene(global::Scene.MAIN_MENU);
        }
    }
}