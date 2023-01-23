using NavySpade.UI.Popups.Abstract;

namespace NavySpade.UI.Popups.DifferentPopups
{
    public class StartGamePopup : Popup
    {
        public static StartGamePopup Instance { get; private set; }

        public override void OnAwake()
        {
            Instance = this;
        }

        public override void OnStart()
        {
            
        }
    }
}