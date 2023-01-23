using Core.Damagables;
using NavySpade.Core.Runtime.Player.Logic;
using QFSW.QC;
using UnityEngine;

namespace NavySpade.Commands.Core
{
    public static class PlayerConsoleCommands
    {
        [Command("player.damageable")]
        public static Damageable GetPlayerDamageable()
        {
            if (SinglePlayer.Instance == null)
            {
                Debug.LogError("player not found");
                return null;
            }
            
            return SinglePlayer.Instance.Damageable;
        }
    }
}