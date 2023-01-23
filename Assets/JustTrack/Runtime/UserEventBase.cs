using System.Collections;
using System.Collections.Generic;

namespace JustTrack {
    public class UserEventBase: UserEvent {
        internal EventDetails Name { get; private set; }
        internal UserEventDimensions Dimensions { get; private set; }
        internal double Value { get; private set; }
        internal Unit? Unit { get; private set; }

        internal UserEventBase(EventDetails pName) {
            this.Name = pName;
            this.Dimensions = new UserEventDimensions();
            this.Value = 0.0;
            this.Unit = null;
        }

        internal void SetDimension(Dimension pDimension, string pValue) {
            this.Dimensions.Set(pDimension, pValue);
        }

        internal void SetValueAndUnit(double pValue, Unit pUnit) {
            this.Value = pValue;
            this.Unit = pUnit;
        }

        internal string GetDimension(Dimension pDimension) {
            return this.Dimensions.Get(pDimension);
        }

        public UserEvent SetDimension1(string pDimension1) {
            this.SetDimension(Dimension.CUSTOM_1, pDimension1);

            return this;
        }

        public UserEvent SetDimension2(string pDimension2) {
            this.SetDimension(Dimension.CUSTOM_2, pDimension2);

            return this;
        }
        public UserEvent SetDimension3(string pDimension3) {
            this.SetDimension(Dimension.CUSTOM_3, pDimension3);

            return this;
        }

        public UserEventBase GetBase() {
            return this;
        }
    }
}
