using System;

namespace BattleBall.Scripts.Events
{
    public class EventMainMenu
    {
        public MainMenu mainMenu;

        public EventMainMenu(MainMenu mainMenu)
        {
            this.mainMenu = mainMenu;
        }

        public void OnLocalMode(object sender, EventArgs e)
        {
            mainMenu.Game.gameSceneManager.LoadScene(global::Scene.LOCAL_MODE);
        }
        public void OnLanMode(object sender, EventArgs e)
        {
            mainMenu.Game.gameSceneManager.LoadScene(global::Scene.LAN_MODE);
        }
        public void OnClientMode(object sender, EventArgs e)
        {
            mainMenu.Game.gameSceneManager.LoadScene(global::Scene.CLIENT_MODE);
        }
        public void OnQuit(object sender, EventArgs e)
        {
            mainMenu.Game.Exit();
        }
        public void OnAbout(object sender, EventArgs e)
        {
            mainMenu.Game.gameSceneManager.LoadScene(global::Scene.ABOUT);
        }
    }
}