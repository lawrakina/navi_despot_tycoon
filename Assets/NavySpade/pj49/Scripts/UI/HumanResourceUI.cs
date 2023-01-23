using NavySpade.Core.Runtime.Player.Logic;

namespace NavySpade.pj49.Scripts.UI
{
    public class HumanResourceUI : ResourceUI
    {
        private UnitsCapacityHandler _unitsCapacity;

        protected override void Init(SinglePlayer player)
        {
            base.Init(player);
            _unitsCapacity = player.CapacityHandler;
            _unitsCapacity.Upgraded += UpdateUi;
        }

        private void UpdateUi()
        {
            CheckUIState();
            UpdateCount();
        }
    }
}