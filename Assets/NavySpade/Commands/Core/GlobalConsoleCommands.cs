using NavySpade.Core.Runtime.Player.Logic;
using QFSW.QC;
using UnityEngine;

namespace NavySpade.Commands.Core
{
    [CommandPrefix("g.")]
    public static class GlobalConsoleCommands
    {
        [Command("player.kill")]
        public static void KillPlayer()
        {
            if (SinglePlayer.Instance == null)
            {
                Debug.LogError("Player not found");
                return;
            }

            SinglePlayer.Instance.Damageable.ForceKill();
        }

        [Command("player.immortal")]
        public static bool IsPlayerImmortal
        {
            get => SinglePlayer.Instance.Damageable.IsImmortal;
            set => SinglePlayer.Instance.Damageable.IsImmortal = value;
        }
    }
}