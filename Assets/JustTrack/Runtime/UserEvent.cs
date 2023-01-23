using System.Collections;
using System.Collections.Generic;

namespace JustTrack {
    public interface UserEvent {
        UserEvent SetDimension1(string pDimension1);
        UserEvent SetDimension2(string pDimension2);
        UserEvent SetDimension3(string pDimension3);
        UserEventBase GetBase();
    }
}
