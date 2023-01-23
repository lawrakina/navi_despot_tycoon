﻿#if UNITY_ANDROID
using System;
using UnityEngine;

namespace JustTrack {
    internal class PreliminaryRetargetingParametersListener : AndroidJavaProxy {
        private const string CLASS = "PreliminaryRetargetingParametersListener";

        internal PreliminaryRetargetingParametersListener(Action<AndroidJavaObject> pOnPreliminaryRetargetingParametersReceived) : base($"{SDKAndroidAgent.PACKAGE}.{CLASS}") {
            m_OnPreliminaryRetargetingParametersReceived = pOnPreliminaryRetargetingParametersReceived;
        }

        Action<AndroidJavaObject> m_OnPreliminaryRetargetingParametersReceived;

        /**
        * Called every time we retrieve new preliminary retargeting parameters.
        *
        * @param preliminaryRetargetingParameters The current Preliminary retargeting parameters of the user.
        */
        void onPreliminaryRetargetingParametersReceived(AndroidJavaObject pParameters) {
            m_OnPreliminaryRetargetingParametersReceived(pParameters);
        }
    }
}
#endif
