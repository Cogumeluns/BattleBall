using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Scene;

namespace BattleBall.Scripts.Events
{
    public class EventGameMode
    {
        public GameMode gameMode;

        public EventGameMode(GameMode GameMode)
        {
            this.gameMode = GameMode;
        }

        public void OnPlayAgain(object sender, EventArgs e)
        {
            gameMode.Game.gameSceneManager.LoadScene(global::Scene.GAME_MODE);
        }

        public void OnMainMenu(object sender, EventArgs e)
        {
            gameMode.Game.gameSceneManager.LoadScene(global::Scene.MAIN_MENU);
        }
    }
}