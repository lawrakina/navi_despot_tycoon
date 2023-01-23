using Core.Damagables;
using QFSW.QC;

namespace NavySpade.Commands.Damagables
{
    [CommandPrefix("game.damageable.")]
    public static class DamageableConsoleCommands
    {
        [Command("kill")]
        public static void Kill(Damageable damageable)
        {
            damageable.ForceKill();
        }
    }
}