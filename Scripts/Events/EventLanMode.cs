using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleBall.Scripts.Events
{
    public class EventLanMode
    {
        LanMode lanMode;
        public EventLanMode(LanMode lanMode)
        {
            this.lanMode = lanMode;
        }

        public void OnStartGameMode(object sender, EventArgs e)
        {
            Task.Run(async () => {
                await GameStatics.connection.SendOpenLanMode();
            });
        }
    }
}