using EventSystem.Runtime.Core.Managers;
using QFSW.QC;

namespace NavySpade.Commands.Core
{
    [CommandPrefix("events.")]
    public static class GameEventsCommands
    {
        [Command("win")]
        public static void Win()
        {
            EventManager.Invoke(GameStatesEM.OnWin);
        }

        [Command("lose")]
        public static void Lose()
        {
            EventManager.Invoke(GameStatesEM.OnFail);
        }
    }
}