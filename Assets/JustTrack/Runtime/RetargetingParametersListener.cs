#if UNITY_ANDROID
using System;
using UnityEngine;

namespace JustTrack {
    internal class RetargetingParametersListener : AndroidJavaProxy {
        private const string CLASS = "RetargetingParametersListener";

        internal RetargetingParametersListener(Action<AndroidJavaObject> pOnRetargetingParametersReceived) : base($"{SDKAndroidAgent.PACKAGE}.{CLASS}") {
            m_OnRetargetingParametersReceived = pOnRetargetingParametersReceived;
        }

        Action<AndroidJavaObject> m_OnRetargetingParametersReceived;

        /**
        * Called every time we retrieve new retargeting parameters.
        *
        * @param retargetingParameters The current retargeting parameters of the user.
        */
        void onRetargetingParametersReceived(AndroidJavaObject pParameters) {
            m_OnRetargetingParametersReceived(pParameters);
        }
    }
}
#endif
