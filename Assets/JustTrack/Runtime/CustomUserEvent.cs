using System.Collections;
using System.Collections.Generic;

namespace JustTrack {
    public class CustomUserEvent: UserEventBase {
        public CustomUserEvent(string pName) : base(new EventDetails(pName)) {
        }

        public CustomUserEvent(EventDetails pName) : base(pName) {
        }

        public new CustomUserEvent SetValueAndUnit(double pValue, Unit pUnit) {
            base.SetValueAndUnit(pValue, pUnit);

            return this;
        }
    }
}
