using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleBall.Scripts.Events
{
    public class EventLocalMode
    {
        LocalMode localMode;
        public EventLocalMode(LocalMode localMode)
        {
            this.localMode = localMode;
        }

        public void OnStartGameMode(object sender, EventArgs e)
        {
            localMode.Game.gameSceneManager.LoadScene(global::Scene.GAME_MODE);
        }
    }
}