
namespace JustTrack {
    public class EventDetails {
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Element { get; private set; }
        public string Action { get; private set; }

        /**
        * Create a new name for a user event.
        *
        * @param pName The name for the user event.
        */
        public EventDetails(string pName) {
            this.Name = pName;
            this.Category = null;
            this.Element = null;
            this.Action = null;
        }

        /**
        * Create a new name for a user event.
        *
        * @param pName The name for the user event.
        * @param pCategory The category for the user event.
        * @param pElement The element for the user event.
        * @param pAction The action for the user event.
        */
        public EventDetails(string pName, string pCategory, string pElement, string pAction) {
            this.Name = pName;
            this.Category = pCategory;
            this.Element = pElement;
            this.Action = pAction;
        }

        internal EventDetails(string pCategory, string pElement, string pAction) {
            this.Name = $"{pCategory}_{pElement}_{pAction}";
            this.Category = pCategory;
            this.Element = pElement;
            this.Action = pAction;
        }
    }
}
