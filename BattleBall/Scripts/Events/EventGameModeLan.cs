using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleBall.Scripts.Scene;

namespace BattleBall.Scripts.Events
{
    public class EventGameModeLan
    {
        public GameModeLan gameModeLan;

        public EventGameModeLan(GameModeLan gameModeLan)
        {
            this.gameModeLan = gameModeLan;
        }

        public void OnPlayAgain(object sender, EventArgs e)
        {
            Task.Run(async () => {
                await GameStatics.connection.SendOpenLanMode();
            });
        }

        public void OnMainMenu(object sender, EventArgs e)
        {
            Task.Run(async () => {
                await GameStatics.connection.SendOpenMainMenu();
            });
        }
    }
}